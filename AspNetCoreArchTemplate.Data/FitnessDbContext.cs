namespace AspNetCoreArchTemplate.Data
{
    using FitnessPlatform.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class FitnessDbContext : IdentityDbContext<ApplicationUser,IdentityRole,string>
    {
        public FitnessDbContext(DbContextOptions<FitnessDbContext> options)
            : base(options)
        {

        }
        public virtual DbSet<Gym> Gym { get; set; }

        public virtual DbSet<GymImage> GymImage { get; set; }

        public DbSet<DailyLog> DailyLog { get; set; }

        public DbSet<Food> Food { get; set; }

        public DbSet<WorkoutSession> WorkoutSession { get; set; }

        public DbSet<UserGymSubscription> UserGymSubscription { get; set; }

        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }


        public DbSet<Event> Events { get; set; }

        public DbSet<Trainer> Trainers { get; set; }

        public DbSet<Specialty> Specialties { get; set; }

        //public DbSet<TrainerSchedule> TrainerSchedules { get; set; }

        public DbSet<GymWorkingHours> GymWorkingHours { get; set; }

        public DbSet<EventRegistration> EventRegistrations { get; set; }
        public DbSet<TrainerClient> TrainerClients { get; set; }

        public DbSet<WorkoutEntry> WorkoutEntries { get; set; }

        public DbSet<WorkoutSession> WorkoutSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            

            builder.Entity<UserGymSubscription>().HasQueryFilter(ugs => ugs.Gym.IsDeleted == false);

            builder.Entity<Event>().HasQueryFilter(e => e.Gym.IsDeleted == false);

            builder.Entity<Trainer>().HasQueryFilter(t => t.Gym.IsDeleted == false);

            builder.Entity<GymImage>().HasQueryFilter(t => t.Gym.IsDeleted == false);

            builder.Entity<UserGymSubscription>().HasKey(ur => new { ur.UserId, ur.GymId });

            builder.Entity<Gym>().HasQueryFilter(g => g.IsDeleted == false);

            builder.Entity<Event>()
            .HasOne(e => e.Gym)
            .WithMany(g => g.Events)
            .HasForeignKey(e => e.GymId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Event>()
                .HasOne(e => e.Trainer)
                .WithMany(t => t.Events)
                .HasForeignKey(e => e.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<UserGymSubscription>()
                .HasOne(ur => ur.User)
                .WithMany(ur=>ur.GymSubscriptions)
                .HasForeignKey(ur => ur.UserId);

            builder.Entity<UserGymSubscription>()
               .HasOne(ur => ur.Gym)
               .WithMany(ur => ur.Subscribers)
               .HasForeignKey(ur => ur.GymId);

            builder.Entity<EventRegistration>().HasKey(ur => new { ur.UserId, ur.EventId });

            builder.Entity<EventRegistration>()
                .HasOne(ur => ur.User)
                .WithMany(ur => ur.EventRegistrations)
                .HasForeignKey(ur => ur.UserId);

            builder.Entity<EventRegistration>()
               .HasOne(ur => ur.Event)
               .WithMany(ur => ur.Registrations)
               .HasForeignKey(ur => ur.EventId);

            builder.Entity<TrainerClient>().HasKey(ur => new { ur.ClientId, ur.TrainerId });

            builder.Entity<TrainerClient>()
                .HasOne(ur => ur.Client)
                .WithMany(ur => ur.AssignedTrainers)
                .HasForeignKey(ur => ur.ClientId);

            builder.Entity<TrainerClient>()
               .HasOne(ur => ur.Trainer)
               .WithMany(ur => ur.Clients)
               .HasForeignKey(ur => ur.TrainerId);


        }
    }
}
