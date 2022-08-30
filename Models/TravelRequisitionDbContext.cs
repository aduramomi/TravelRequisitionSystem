using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelRequisitionSystem.Models
{
    public class TravelRequisitionDbContext : DbContext
    {
        public TravelRequisitionDbContext(DbContextOptions<TravelRequisitionDbContext> options) : base(options)
        {

        }

        public DbSet<Requisition> Requisitions { get; set; }
    }
}
