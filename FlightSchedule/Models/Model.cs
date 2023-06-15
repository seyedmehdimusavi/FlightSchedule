using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightSchedule.model
{

    public class FlightScheduleContext : DbContext
    {
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=srv-b1-db.dadekavan.lan; Initial Catalog=mehdi_flightS;User Id=Dadekavan; Password=DK#DB10;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


        public DbSet<route> routes { get; set; }
        public DbSet<flight> flights { get; set; }
        public DbSet<subscription> subscriptions { get; set; }

        public class route
        {
            [Key]
            //public int id { get; set; }
            public int route_id { get; set; }
            public int origin_city_id { get; set; }
            public int destination_city_id { get; set; }
            public string departure_date { get; set; }
        }

        public class flight
        {
            // public int id { get; set; }
            [Key]
            public int flight_id { get; set; }
            public int route_id { get; set; }
            public DateTime departure_time { get; set; }
            public DateTime arrival_time { get; set; }
            public int airline_id { get; set; }
            public Int64? departureValue { get; set; }
            public ICollection<route>? routes { get; set; }
        }


        public class subscription
        {
            public int id { get; set; }
            public int agency_id { get; set; }
            public int origin_city_id { get; set; }
            public int destination_city_id { get; set; }
        }
     }
}
