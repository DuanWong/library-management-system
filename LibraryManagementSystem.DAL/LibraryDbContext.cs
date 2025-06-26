using LibraryManagementSystem.Common;
using LibraryManagementSystem.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.DAL
{
    public class LibraryDbContext : IdentityDbContext<IdentityUser>
    {
        public LibraryDbContext() { }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<Editorial> Editorials { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanDetail> LoanDetails { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Primary Keys.
            modelBuilder.Entity<Author>()
                .HasKey(a => a.AuthorId);

            modelBuilder.Entity<Book>()
                .HasKey(b => b.BookId);

            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => ba.BookAuthorId);

            modelBuilder.Entity<Edition>()
                .HasKey(e => e.EditionId);

            modelBuilder.Entity<Editorial>()
                .HasKey(e => e.EditorialId);

            modelBuilder.Entity<LoanDetail>()
                .HasKey(ld => ld.LoanDetailId);

            modelBuilder.Entity<Loan>()
                .HasKey(l => l.LoanId);

            modelBuilder.Entity<Rating>()
                .HasKey(r => r.RatingId);

            modelBuilder.Entity<Subject>()
                .HasKey(s => s.SubjectId);

            modelBuilder.Entity<Reader>()
                .HasKey(r => r.ReaderId);

            // Unique constraints.
            modelBuilder.Entity<BookAuthor>()
                .HasIndex(ba => new { ba.AuthorId, ba.BookId })
                .IsUnique();

            modelBuilder.Entity<Subject>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<Reader>()
                .HasIndex(r => r.CoreId)
                .IsUnique();

            modelBuilder.Entity<Reader>()
                .HasIndex(r => r.Phone)
                .IsUnique();

            modelBuilder.Entity<Edition>()
                .HasIndex(e => e.ISBN)
                .IsUnique();

            modelBuilder.Entity<Editorial>()
                .HasIndex(e => e.Name)
                .IsUnique();

            modelBuilder.Entity<LoanDetail>()
                .HasIndex(ld => new { ld.LoanId, ld.BookId })
                .IsUnique();

            // Required and length restrictions.
            modelBuilder.Entity<Author>()
                      .Property(a => a.FirstName)
                      .HasMaxLength(30)
                      .IsRequired();

            modelBuilder.Entity<Author>()
                      .Property(a => a.LastName)
                      .HasMaxLength(30)
                      .IsRequired();

            modelBuilder.Entity<Author>()
                      .Property(a => a.BirthDate)
                      .IsRequired();

            modelBuilder.Entity<Author>()
                      .Property(a => a.Biography)
                      .HasMaxLength(500);

            modelBuilder.Entity<Edition>()
                      .Property(e => e.ISBN)
                      .HasMaxLength(13)
                      .IsRequired();

            modelBuilder.Entity<Subject>()
                      .Property(s => s.Name)
                      .HasMaxLength(30)
                      .IsRequired();

            modelBuilder.Entity<Book>()
                      .Property(b => b.Title)
                      .HasMaxLength(50)
                      .IsRequired();

            modelBuilder.Entity<Book>()
                      .Property(b => b.SubjectId)
                      .IsRequired();

            modelBuilder.Entity<Book>()
                      .Property(b => b.Synopsis)
                      .HasMaxLength(500);

            modelBuilder.Entity<Book>()
                      .Property(b => b.Photo)
                      .HasMaxLength(300);

            modelBuilder.Entity<BookAuthor>()
                      .Property(b => b.BookId)
                      .IsRequired();

            modelBuilder.Entity<BookAuthor>()
                      .Property(b => b.AuthorId)
                      .IsRequired();

            modelBuilder.Entity<Reader>()
                      .Property(r => r.CoreId)
                      .IsRequired()
                      .HasMaxLength(450);

            modelBuilder.Entity<Reader>()
                      .Property(r => r.FirstName)
                      .IsRequired()
                      .HasMaxLength(30);

            modelBuilder.Entity<Reader>()
                      .Property(r => r.LastName)
                      .IsRequired()
                      .HasMaxLength(30);

            modelBuilder.Entity<Reader>()
                      .Property(r => r.Email)
                      .IsRequired()
                      .HasMaxLength(100);

            modelBuilder.Entity<Reader>()
                      .Property(r => r.Phone)
                      .HasMaxLength(15);

            modelBuilder.Entity<Editorial>()
                      .Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(30);

            modelBuilder.Entity<Edition>()
                      .Property(e => e.BookId)
                      .IsRequired();

            modelBuilder.Entity<Edition>()
                      .Property(e => e.EditorialId)
                      .IsRequired();

            modelBuilder.Entity<Edition>()
                      .Property(e => e.EditionDate)
                      .IsRequired();

            modelBuilder.Entity<Loan>()
                      .Property(l => l.ReaderId)
                      .IsRequired();

            modelBuilder.Entity<Loan>()
                      .Property(l => l.InitialDate)
                      .IsRequired();

            modelBuilder.Entity<Loan>()
                      .Property(l => l.FinalDate)
                      .IsRequired();

            modelBuilder.Entity<LoanDetail>()
                      .Property(ld => ld.LoanId)
                      .IsRequired();

            modelBuilder.Entity<LoanDetail>()
                      .Property(ld => ld.BookId)
                      .IsRequired();

            modelBuilder.Entity<Rating>()
                      .Property(r => r.BookId)
                      .IsRequired();

            modelBuilder.Entity<Rating>()
                      .Property(r => r.ReaderId)
                      .IsRequired();

            // Check constraints.
            modelBuilder.Entity<Author>()
                .ToTable("Authors", tableBuilder =>
                {
                    string checkBirthDateAuthor = $"BirthDate <= DATEADD(YEAR, -{Constants.MINIMUN_AGE_TOBE_AUTHOR}, GETDATE())";
                    tableBuilder.HasCheckConstraint("CK_Author_BirthDate", checkBirthDateAuthor);
                    tableBuilder.HasCheckConstraint("CK_Author_DateOfDeath", "DateOfDeath IS NULL OR DateOfDeath <= GETDATE()");
                    tableBuilder.HasCheckConstraint("CK_Author_BirthDate_DateOfDeath", "DateOfDeath IS NULL OR BirthDate < DateOfDeath");
                });

            modelBuilder.Entity<Edition>()
                .ToTable("Editions", tableBuilder =>
                {
                    tableBuilder.HasCheckConstraint("CK_Edition_EditionDate", "EditionDate IS NULL OR EditionDate <= GETDATE()");
                });

            modelBuilder.Entity<Loan>()
                .ToTable("Loans", tableBuilder =>
                {
                    tableBuilder.HasCheckConstraint("CK_Loan_InitialDate", "InitialDate <= GETDATE()");
                    tableBuilder.HasCheckConstraint("CK_Loan_InitialDate_FinalDate", "InitialDate <= FinalDate");
                });

            modelBuilder.Entity<Reader>()
                .ToTable("Readers", tableBuilder =>
                {
                    string checkBirthDateReader = $"BirthDate IS NULL OR BirthDate <= DATEADD(YEAR, -{Constants.MINIMUN_AGE_TOBE_READER}, GETDATE())";
                    tableBuilder.HasCheckConstraint("CK_Reader_BirthDay", checkBirthDateReader);
                });

            // Relationships.
            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Edition>()
                .HasOne(e => e.Book)
                .WithMany(b => b.Editions)
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Edition>()
                .HasOne(e => e.Editorial)
                .WithMany(e => e.Editions)
                .HasForeignKey(e => e.EditorialId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Subject)
                .WithMany(s => s.Books)
                .HasForeignKey(b => b.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoanDetail>()
                .HasOne(ld => ld.Book)
                .WithMany(b => b.LoanDetails)
                .HasForeignKey(ld => ld.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoanDetail>()
                .HasOne(ld => ld.Loan)
                .WithMany(l => l.LoanDetails)
                .HasForeignKey(ld => ld.LoanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Reader)
                .WithMany(r => r.Ratings)
                .HasForeignKey(r => r.ReaderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Ratings)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Reader)
                .WithMany(r => r.Loans)
                .HasForeignKey(l => l.ReaderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}