using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    //DbContext will provide all the logic that EF Core is going to use to do it's change tracking and data base interaction tasks.
    public class SamuraiContext : DbContext
    {
        //CONSTRUCTOR - takes in the DbContext options
        //The internal DbContext class already has a constructor like this,
        //add 'base' so it'll do whatever the base class's matching constructor already knows how to do with the 'options'
        public SamuraiContext(DbContextOptions<SamuraiContext> options) : base(options)
        {
            //Disable tracking all together
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            //If you add logic that needs query results to be tracked within a particular method, you would have to change your strategy
        }

        //A DbContext needs to expose DbSets, which become wrappers to the different types that you'll interact with while you're using the context.
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }
        //EF Core will presume that the table names match these DbSet names.

        //Using the Fluent API to specify the last critical detail of the many-to-many relationship.
        //he fluent mappings go into DbContext's onModelCreating method, which gets called internally at runtime when EF Core is working out what the data model looks like.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Using the modelBuilder object that EF Core has passed into the method, it's told that the SamuraiBattle Entity has a key that's composed from its SamuraiId & BattleId properites.
            modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });
            
            //Tell the modelBuilder that the Horse entity maps to a table called Horses
            modelBuilder.Entity<Horse>().ToTable("Horses");

            //Configure context to be aware SamuraiBattleStats intentionally doesn't have a key so EF Core doesn't get upset
            //Add ToView() so context is aware there's already a view, or else EF Core will think it needs to create a new db table 
            modelBuilder.Entity<SamuraiBattleStat>().HasNoKey().ToView("SamuraiBattleStats");
            //EF Core will never track entities marked with HasNoKey()
        }
    }
}
