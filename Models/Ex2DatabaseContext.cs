using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AlbumSong.Models;

public partial class Ex2DatabaseContext : DbContext
{
    public Ex2DatabaseContext()
    {
    }

    public Ex2DatabaseContext(DbContextOptions<Ex2DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-MF38127;Initial Catalog=Ex2_database;Integrated Security=True;Pooling=False;Encrypt=False;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasOne(d => d.File).WithMany(p => p.Albums).HasConstraintName("FK_Album_Id_File_Id");
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasOne(d => d.Album).WithMany(p => p.Songs).HasConstraintName("FK_Album_ID__Song_AlbumID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
