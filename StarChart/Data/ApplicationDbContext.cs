using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using StarChart.Models;

namespace StarChart.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CelestialObject> CelestialObjects { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=CelestialData";
            optionsBuilder.UseSqlServer(connectionString);
        }

        public CelestialObject GetByIdWithSatellites(int id)
        {
            var star = Find<CelestialObject>(id);
            return AddSatellites(star);
        }
        
        public List<CelestialObject> GetByNameWithSatellites(string name)
        {
            var query = from s in CelestialObjects
                       where s.Name == name
                       select s;
            var stars = query.ToList();
            foreach (var s in stars)
            {
                AddSatellites(s);
            }
            return stars;
        }

        public List<CelestialObject> GetAllWithSatellites()
        {
            var allStars = CelestialObjects.ToList();
            foreach (var s in allStars)
            {
                AddSatellites(s);
            }
            return allStars;
        }

        private CelestialObject AddSatellites(CelestialObject star)
        {
            if (star == null) return star;
            var satellites = from s in CelestialObjects
                             where s.OrbitedObjectId == star.Id
                             select s;
            star.Satellites = satellites.ToList();
            return star;
        }
    }
}
