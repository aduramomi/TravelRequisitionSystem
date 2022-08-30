using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TravelRequisitionSystem.Helper
{
    public class Utility
    {
        public static bool IsJsonValid(string input)
        {
            try
            {
                //input = input.Trim().Replace("'", "\"");

                var output = System.Json.JsonValue.Parse(input);

                return true;
            }
            catch (Exception eX)  // Typically a JsonReaderException exception if you want to specify.
            {
                return false;
            }
        }

        public static TResult Deserialize<TResult>(Stream responseStream, bool EnforceMissingMemberHandling = false, bool IgnoreNullValues = false)
        {
            using (var sr = new StreamReader(responseStream))
            {
                using (var reader = new JsonTextReader(sr))
                {
                    var serializer = new JsonSerializer
                    {
                        MissingMemberHandling = EnforceMissingMemberHandling ? MissingMemberHandling.Error : MissingMemberHandling.Ignore,
                        NullValueHandling = IgnoreNullValues ? NullValueHandling.Ignore : NullValueHandling.Include
                    };

                    return serializer.Deserialize<TResult>(reader);
                }
            }
        }
    }
}
