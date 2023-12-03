using Fujitsu.CvQc.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fujitsu.CvQc.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO FIXME Temporary solution to prevent problems with relationship navigation. (See story #133 for more details)
        modelBuilder.Entity<Project>().Navigation(e => e.Jobs).AutoInclude();
        modelBuilder.Entity<Project>().Navigation(e => e.Config).AutoInclude();
        modelBuilder.Entity<Job>().Navigation(e => e.Logs).AutoInclude();
        modelBuilder.Entity<Project>().Navigation(p => p.Documents).AutoInclude();
        modelBuilder.Entity<DocumentMap>().Navigation(m => m.Sections).AutoInclude();
        modelBuilder.Entity<DocumentMapSection>().Navigation(s => s.Paragraphs).AutoInclude();
    }

    public DbSet<Project> Projects { get; set; } = null!;

    public DbSet<Document> Documents { get; set; } = null!;

    public DbSet<DocumentMap> DocumentMaps { get; set; } = null!;

    public DbSet<DocumentMapSection> Sections { get; set; } = null!;

    public DbSet<SectionParagraph> Paragraphs { get; set; } = null!;

    public DbSet<Job> Jobs { get; set; } = null!;

    public DbSet<Log> Logs { get; set; } = null!;
}
