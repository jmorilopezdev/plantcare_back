using Microsoft.EntityFrameworkCore;
using PlantCareBackEnd.Models;

namespace PlantCareBackEnd.Data
{
    public class PlantDbContext: DbContext
    {
        public PlantDbContext(DbContextOptions<PlantDbContext> options) : base(options) { }

        public DbSet<Plant> Plants { get; set; }
    }
}
