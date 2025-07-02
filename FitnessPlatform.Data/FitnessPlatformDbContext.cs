namespace AspNetCoreArchTemplate.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class FitnessPlatformDbContext : IdentityDbContext
    {
        public FitnessPlatformDbContext(DbContextOptions<FitnessPlatformDbContext> options)
            : base(options)
        {

        }
    }
}
