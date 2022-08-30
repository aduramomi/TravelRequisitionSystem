using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Threading.Tasks;
using TravelRequisitionSystem.Dtos;
using TravelRequisitionSystem.Enums;
using TravelRequisitionSystem.Helper;
using TravelRequisitionSystem.Models;

namespace TravelRequisitionSystem.Services
{
    public class RequestService : IRequestService
    {
        private readonly TravelRequisitionDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly string openWeatherApiKey;
        private readonly string locationCoordApiUrl;
        private readonly string weatherForecastApiUrl;
        private readonly string countryDetailApiUrl;
        private readonly ApiConsumptionHelper _apiConsumptionHelper;

        public RequestService(TravelRequisitionDbContext db, IConfiguration configuration, ApiConsumptionHelper apiConsumptionHelper)
        {
            _db = db;
            _configuration = configuration;
            openWeatherApiKey = _configuration.GetSection("AppSettings:OpenWeatherApiKey").Value;
            locationCoordApiUrl = _configuration.GetSection("AppSettings:LocationCoordApiUrl").Value;
            weatherForecastApiUrl = _configuration.GetSection("AppSettings:WeatherForecastApiUrl").Value;
            countryDetailApiUrl = _configuration.GetSection("AppSettings:CountryDetailApiUrl").Value;
            _apiConsumptionHelper = apiConsumptionHelper;
        }

        public async Task<ServiceResponse<string>> AddRequest(AddRequestDto requestDto)
        {
            string msg = "";
            ServiceResponse<string> response = new ServiceResponse<string>();

            try
            {
                Enums.Country? sourceCountry = null;
                Enums.Country? destinationCountry = null;
                TravelClass? travelClass = null;
                TripType? tripType = null;

                msg = ConvertToEnumValues(requestDto.SourceCountry, requestDto.DestinationCountry, requestDto.TravelClass,
                    requestDto.TripType, out sourceCountry, out destinationCountry, out travelClass, out tripType);

                if (!string.IsNullOrEmpty(msg))
                {
                    response.ErrorMessage = msg;
                    return response;
                }

                Random random = new Random();
                string requisitionNumber = $"TRS{DateTime.Now.ToString("ddMMyyyyHHmmss")}{random.Next(1, 100).ToString().PadLeft(3, '0')}";

                Requisition requisition = new Requisition
                {
                    ChargeCode = requestDto.ChargeCode,
                    DestinationCountry = destinationCountry.Value,
                    DestinationLocation = requestDto.DestinationLocation,
                    ProposedDepartureDateAndTime = DateTime.Parse($"{requestDto.ProposedDepartureDate} {requestDto.ProposedDepartureTime} :00.000"),
                    RequestDate = DateTime.Now,
                    RequisitionNumber = requisitionNumber,
                    RequisitionStatus = RequisitionStatus.submitted,
                    SourceCountry = sourceCountry.Value,
                    SourceLocation = requestDto.SourceLocation,
                    TravelClass = travelClass.Value,
                    TravelerName = requestDto.TravelerName,
                    TripType = tripType.Value
                };

                _db.Requisitions.Add(requisition);
                await _db.SaveChangesAsync();

                response.Data = $"Requisition Number: {requisitionNumber}";
                response.IsSuccessful = true;

                return response;
            }           
            catch (Exception eX)
            {
                msg = eX.Message;

                if (eX.InnerException != null)
                {
                    msg += "; " + eX.InnerException.Message; if (eX.InnerException.InnerException != null) { msg += ";" + eX.InnerException.InnerException.Message; }
                }

                response.ErrorMessage = msg;
            }

            return response;
        }

        private string ConvertToEnumValues(string sourceCountry, string destinationCountry, string travelClass, string tripType, 
            out Enums.Country? _sourceCountry, out Enums.Country? _destinationCountry, out TravelClass? _travelClass, out TripType? _tripType)
        {
            string msg = "";

            object srcCountry = null;
            object destCountry = null;
            object travClass = null;
            object typeOfTrip = null;

            //sourceCountry
            msg += ConvertToEnumValue<Enums.Country>("Source Country", sourceCountry.Replace(" ", "_").Replace("-", "__"), out srcCountry);

            if (srcCountry != null)
                _sourceCountry = (Enums.Country)srcCountry;
            else
                _sourceCountry = null;

            //destinationCountry
            msg += ConvertToEnumValue<Enums.Country>("Destination Country", destinationCountry.Replace(" ", "_").Replace("-", "__"), out destCountry);

            if (destCountry != null)
                _destinationCountry = (Enums.Country)destCountry;
            else
                _destinationCountry = null;

            //travelClass
            msg += ConvertToEnumValue<TravelClass>("Travel Class", travelClass.Replace(" ", "_"), out travClass);

            if (travClass != null)
                _travelClass = (TravelClass)travClass;
            else
                _travelClass = null;

            //tripType
            msg += ConvertToEnumValue<TripType>("Trip Type", tripType.Replace(" ", "_"), out typeOfTrip);

            if (typeOfTrip != null)
                _tripType = (TripType)typeOfTrip;
            else
                _tripType = null;

            return msg;
        }

        private string ConvertToEnumValue<T>(string enumTypeIdentifier, string valueToConvert, out object result)
        {
            if (Enum.TryParse(typeof(T), valueToConvert.ToLower(), out result))
            {
                return "";
            }
            else
            {
                return $"Invalid {enumTypeIdentifier}; ";
            }
        }

        public async Task<ServiceResponse<string>> DeleteRequest(string requisitionNumber)
        {
            string msg = "";
            ServiceResponse<string> response = new ServiceResponse<string>();

            try
            {
                ServiceResponse<RequestDto> request = await GetRequestByRequisitionNumber(requisitionNumber);

                if (!request.IsSuccessful)
                {
                    response.ErrorMessage = request.ErrorMessage;
                    return response;
                }

                var requestToDelete = await _db.Requisitions.Where(m => m.RequisitionNumber.ToUpper() == requisitionNumber.ToUpper()).FirstOrDefaultAsync();

                _db.Requisitions.Remove(requestToDelete);
                await _db.SaveChangesAsync();

                response.IsSuccessful = true;

                return response;                
            }
            catch (Exception eX)
            {
                msg = eX.Message;

                if (eX.InnerException != null)
                {
                    msg += "; " + eX.InnerException.Message; if (eX.InnerException.InnerException != null) { msg += ";" + eX.InnerException.InnerException.Message; }
                }

                response.ErrorMessage = msg;
            }

            return response;
        }

        public async Task<ServiceResponse<RequestDto>> GetRequestByRequisitionNumber(string requisitionNumber)
        {
            string msg = "";
            ServiceResponse<RequestDto> response = new ServiceResponse<RequestDto>();

            try
            {
                var request = await _db.Requisitions.Where(m => m.RequisitionNumber.ToUpper() == requisitionNumber.ToUpper()).FirstOrDefaultAsync();

                if (request == null)
                {
                    response.Data = null;
                    response.ErrorMessage = "Request not found or invalid requisition number";

                    return response;
                }
               
                response.Data = new RequestDto
                {
                    ChargeCode = request.ChargeCode,
                    DestinationCountry = Enum.GetName(typeof(Enums.Country), request.DestinationCountry).ToString().Replace("_", " ").Replace("__", "-"),
                    DestinationLocation = request.DestinationLocation,
                    ProposedDepartureDateAndTime = request.ProposedDepartureDateAndTime.ToString("dd/MM/yyyy HH:mm"),
                    RequestDate = request.RequestDate.ToString("dd/MM/yyyy HH:mm"),
                    RequestStatus = Enum.GetName(typeof(RequisitionStatus), request.RequisitionStatus),
                    RequisitionNumber = request.RequisitionNumber,
                    SourceCountry = Enum.GetName(typeof(Enums.Country), request.SourceCountry).ToString().Replace("_", " ").Replace("__", "-"),
                    SourceLocation = request.SourceLocation,
                    TravelClass = Enum.GetName(typeof(TravelClass), request.TravelClass).ToString().Replace("_", " "),
                    TravelerName = request.TravelerName,
                    TripType = Enum.GetName(typeof(TripType), request.TripType).ToString().Replace("_", " ")
                };

                response.IsSuccessful = true;
                return response;
            }
            catch (Exception eX)
            {
                msg = eX.Message;

                if (eX.InnerException != null)
                {
                    msg += "; " + eX.InnerException.Message; if (eX.InnerException.InnerException != null) { msg += ";" + eX.InnerException.InnerException.Message; }
                }

                response.ErrorMessage = msg;
            }

            return response;
        }

        public async Task<ServiceResponse<string>> UpdateRequest(string requisitionNumber, UpdateRequestDto requestDto)
        {
            string msg = "";
            ServiceResponse<string> response = new ServiceResponse<string>();

            try
            {
                ServiceResponse<RequestDto> request = await GetRequestByRequisitionNumber(requisitionNumber);

                if (!request.IsSuccessful)
                {
                    response.ErrorMessage = request.ErrorMessage;
                    return response;
                }

                //check if request has been booked
                if (request.Data.RequestStatus.ToUpper() == "Booked".ToUpper())
                {
                    response.ErrorMessage = "Request cannot be updated. It has been booked!";
                    return response;
                }

                Enums.Country? sourceCountry = null;
                Enums.Country? destinationCountry = null;
                TravelClass? travelClass = null;
                TripType? tripType = null;

                msg = ConvertToEnumValues(requestDto.SourceCountry, requestDto.DestinationCountry, requestDto.TravelClass,
                    requestDto.TripType, out sourceCountry, out destinationCountry, out travelClass, out tripType);

                if (!string.IsNullOrEmpty(msg))
                {
                    response.ErrorMessage = msg;
                    return response;
                }

                var requestToUpdate = await _db.Requisitions.Where(m => m.RequisitionNumber.ToUpper() == requisitionNumber.ToUpper()).FirstOrDefaultAsync();

                requestToUpdate.ChargeCode = requestDto.ChargeCode;
                requestToUpdate.DestinationCountry = destinationCountry.Value;
                requestToUpdate.DestinationLocation = requestDto.DestinationLocation;
                requestToUpdate.ProposedDepartureDateAndTime = DateTime.Parse($"{requestDto.ProposedDepartureDate} {requestDto.ProposedDepartureTime} :00.000");                
                requestToUpdate.RequisitionNumber = requisitionNumber;               
                requestToUpdate.SourceCountry = sourceCountry.Value;
                requestToUpdate.SourceLocation = requestDto.SourceLocation;
                requestToUpdate.TravelClass = travelClass.Value;
                requestToUpdate.TravelerName = requestDto.TravelerName;
                requestToUpdate.TripType = tripType.Value;
                requestToUpdate.DateUpdated = DateTime.Now;

                await _db.SaveChangesAsync();

                response.IsSuccessful = true;

                return response;
            }
            catch (Exception eX)
            {
                msg = eX.Message;

                if (eX.InnerException != null)
                {
                    msg += "; " + eX.InnerException.Message; if (eX.InnerException.InnerException != null) { msg += ";" + eX.InnerException.InnerException.Message; }
                }

                response.ErrorMessage = msg;
            }

            return response;
        }

        public async Task<ServiceResponse<List<LocationDetail>>> GetLocationDetails(string locationName)
        {
            string msg = "";
            ServiceResponse<List<LocationDetail>> response = new ServiceResponse<List<LocationDetail>>();

            try
            {
                var locations = await GetLocationCoordinates(locationName);

                /*
                 * PLS, I COULD NOT CONTINUE WITH THIS END POINT. TIME WAS NOT ON MY SIDE TO REALLY RESOLVE THE ERROR BELOW I WAS GETTING
                 * ERROR:  "Unexpected character encountered while parsing value: {. Path 'local_names', line 1, position 28."
                 * I UNDERSTOOD WHAT THE ERROR WAS. TO GET RID OF THE UNEXPECTED CHARACTERS COMING FROM http://api.openweathermap.org/
                 * SEE SAMPLE JSON VALUES FROM http://api.openweathermap.org/ CONTAINING UNEXPECTED CHARACTERS
                 * */

                //[{ "name":"Akure","local_names":{ "en":"Akure","ko":"아쿠레","ru":"Акуре","uk":"Акуре","ar":"أكوري"},"lat":7.2525595,"lon":5.1932647,"country":"NG","state":"Ondo"},{ "name":"Akure","lat":5.766667,"lon":6,"country":"NG","state":"Edo"},{ "name":"Akure","lat":7.9595,"lon":8.7607,"country":"NG","state":"Benue"}]
            }
            catch (Exception eX)
            {
                msg = eX.Message;

                if (eX.InnerException != null)
                {
                    msg += "; " + eX.InnerException.Message; if (eX.InnerException.InnerException != null) { msg += ";" + eX.InnerException.InnerException.Message; }
                }

                response.ErrorMessage = msg;
            }

            return response;
        }

        private async Task<Locations> GetLocationCoordinates(string locationName)
        {
            string msg = "";
            Locations response = new Locations();
           
            string url = locationCoordApiUrl.Replace("{location_name}", locationName).Replace("{limit_number}", "3").Replace("{ApiKey}", openWeatherApiKey);
            var result = await _apiConsumptionHelper.ConsumeAPI<Locations>(url, null, ApiMethod.GET, false, null, "");            

            return response;
        }
    }
}
