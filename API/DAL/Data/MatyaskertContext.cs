using API.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace API.DAL.Data;

public partial class MatyaskertContext :DbContext {
    public MatyaskertContext() {
    }

    public MatyaskertContext(DbContextOptions<MatyaskertContext> options)
        : base(options) {
    }

    public virtual DbSet<Bet> Bets { get; set; }

    public virtual DbSet<Contest> Contests { get; set; }

    public virtual DbSet<Incontest> Incontests { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Scoringrule> Scoringrules { get; set; }

    public virtual DbSet<Standing> Standings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=localhost;user=Tesztuser;password=Blazor12345;database=matyaskert");

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Bet>(entity => {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("bet");

            entity.HasIndex(e => e.ContestId, "contestId");

            entity.HasIndex(e => e.MatchId, "matchId");

            entity.HasIndex(e => e.UserId, "userId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContestId).HasColumnName("contestId");
            entity.Property(e => e.GuestGoals).HasColumnName("guestGoals");
            entity.Property(e => e.HomeGoals).HasColumnName("homeGoals");
            entity.Property(e => e.IsWon)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isWon");
            entity.Property(e => e.MatchId).HasColumnName("matchId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Contest).WithMany(p => p.Bets)
                .HasForeignKey(d => d.ContestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bet_ibfk_1");

            entity.HasOne(d => d.Match).WithMany(p => p.Bets)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bet_ibfk_3");

            entity.HasOne(d => d.User).WithMany(p => p.Bets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bet_ibfk_2");
        });

        modelBuilder.Entity<Contest>(entity => {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("contest");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.IsOpened)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isOpened");
            entity.Property(e => e.Name)
                .HasMaxLength(256)
                .HasColumnName("name");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");
        });

        modelBuilder.Entity<Incontest>(entity => {
            entity
                .HasNoKey()
                .ToTable("incontest");

            entity.HasIndex(e => e.ContestId, "contestId");

            entity.HasIndex(e => e.MatchId, "matchId");

            entity.Property(e => e.ContestId).HasColumnName("contestId");
            entity.Property(e => e.MatchId).HasColumnName("matchId");

            entity.HasOne(d => d.Contest).WithMany()
                .HasForeignKey(d => d.ContestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("incontest_ibfk_1");

            entity.HasOne(d => d.Match).WithMany()
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("incontest_ibfk_2");
        });

        modelBuilder.Entity<Match>(entity => {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("match");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.GuestGoals).HasColumnName("guestGoals");
            entity.Property(e => e.GuestName)
                .HasMaxLength(256)
                .HasColumnName("guestName");
            entity.Property(e => e.HomeGoals).HasColumnName("homeGoals");
            entity.Property(e => e.HomeName)
                .HasMaxLength(256)
                .HasColumnName("homeName");
            entity.Property(e => e.IsAvailable)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isAvailable");
        });

        modelBuilder.Entity<Scoringrule>(entity => {
            entity
                .HasNoKey()
                .ToTable("scoringrules");

            entity.HasIndex(e => e.ContestId, "contestId");

            entity.Property(e => e.ContestId).HasColumnName("contestId");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
            entity.Property(e => e.Points).HasColumnName("points");

            entity.HasOne(d => d.Contest).WithMany()
                .HasForeignKey(d => d.ContestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("scoringrules_ibfk_1");
        });

        modelBuilder.Entity<Standing>(entity => {
            entity
                .HasNoKey()
                .ToTable("standings");

            entity.HasIndex(e => e.ContestId, "contestId");

            entity.HasIndex(e => e.UserId, "userId");

            entity.Property(e => e.ContestId).HasColumnName("contestId");
            entity.Property(e => e.Points)
                .HasDefaultValueSql("'0'")
                .HasColumnName("points");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Contest).WithMany()
                .HasForeignKey(d => d.ContestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("standings_ibfk_1");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("standings_ibfk_2");
        });

        modelBuilder.Entity<User>(entity => {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isActive");
            entity.Property(e => e.IsAdmin)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isAdmin");
            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .HasColumnName("password");
            entity.Property(e => e.RealName)
                .HasMaxLength(256)
                .HasColumnName("realName");
            entity.Property(e => e.Username)
                .HasMaxLength(256)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
