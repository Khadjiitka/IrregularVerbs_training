using IrregularVerbsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace IrregularVerbsApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<IrregularVerb> Verbs => Set<IrregularVerb>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IrregularVerb>().HasData(
            new IrregularVerb { Id = 1, Infinitive = "be", PastSimple = "was/were", PastParticiple = "been", Translation = "быть, являться", Difficulty = 1 },
            new IrregularVerb { Id = 2, Infinitive = "go", PastSimple = "went", PastParticiple = "gone", Translation = "идти, ехать", Difficulty = 1 },
            new IrregularVerb { Id = 3, Infinitive = "take", PastSimple = "took", PastParticiple = "taken", Translation = "брать, брать с собой", Difficulty = 1 },
            new IrregularVerb { Id = 4, Infinitive = "see", PastSimple = "saw", PastParticiple = "seen", Translation = "видеть", Difficulty = 1 },
            new IrregularVerb { Id = 5, Infinitive = "make", PastSimple = "made", PastParticiple = "made", Translation = "делать, создавать", Difficulty = 1 },
            new IrregularVerb { Id = 6, Infinitive = "know", PastSimple = "knew", PastParticiple = "known", Translation = "знать", Difficulty = 1 },
            new IrregularVerb { Id = 7, Infinitive = "write", PastSimple = "wrote", PastParticiple = "written", Translation = "писать", Difficulty = 2 },
            new IrregularVerb { Id = 8, Infinitive = "sing", PastSimple = "sang", PastParticiple = "sung", Translation = "петь", Difficulty = 2 }
        );
    }
}