using Microsoft.EntityFrameworkCore;

namespace OrganicFertilizerRecomondation.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CropType> cropTypes { get; set; }
        public DbSet<CropAge>  cropAges { get; set; }
        public DbSet<NaturalSource>  naturalSources { get; set; }
    }
}
