using AlumniManagementSystem.Domain;
using AlumniManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
 
namespace AlumniManagementSystem.Infrastructure;
 
public class ApplicationDbContext : DbContext{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
 
  // DbSets
  public DbSet<User> Users { get; set; }
  public DbSet<Alumni> Alumnis { get; set; }
  public DbSet<Department> Departments { get; set; }
  public DbSet<AcademicProgram> Programs { get; set; }
  public DbSet<Event> Events { get; set; }
  public DbSet<EventRSVP> EventRSVPs { get; set; }
  public DbSet<JobPosting> JobPostings { get; set; }
  public DbSet<Donation> Donations { get; set; }
  public DbSet<Newsletter> Newsletters { get; set; }
  public DbSet<NewsletterRecipient> NewsletterRecipients { get; set; }
  public DbSet<Message> Messages { get; set; }
  public DbSet<Survey> Surveys { get; set; }
  public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
  public DbSet<SurveyResponse> SurveyResponses { get; set; }
  public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
  public DbSet<Contact> Contacts { get; set; }
 
  protected override void OnModelCreating(ModelBuilder modelBuilder){
    base.OnModelCreating(modelBuilder);
 
    // User
    modelBuilder.Entity<User>(e => {
      e.HasKey(u => u.UserId);
      e.HasIndex(u => u.Email).IsUnique();
      e.Property(u => u.Role).HasConversion<string>();
    });
 
    // Alumni
    modelBuilder.Entity<Alumni>(e => {
      e.HasKey(a => a.AlumniId);
      e.HasIndex(a => a.RollNumber).IsUnique();
      e.Property(a => a.Gender).HasConversion<string>();
 
      // 1:1 Alumni -> User
      e.HasOne(a => a.User).WithOne(u => u.AlumniProfile).HasForeignKey<Alumni>(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
 
      // M:1 Alumni -> Program
      e.HasOne(a => a.Program).WithMany(p => p.Alumnis).HasForeignKey(a => a.ProgramId).OnDelete(DeleteBehavior.Restrict);
    });
 
    // Department
    modelBuilder.Entity<Department>(e => {
      e.HasKey(d => d.DepartmentId);
      e.HasIndex(d => d.Code).IsUnique();
    });
 
    // AcademicProgram
    modelBuilder.Entity<AcademicProgram>(e => {
      e.HasKey(p => p.ProgramId);
      e.ToTable("Programs"); // table name stays "Programs" in MySQL
 
      // M:1 Program -> Department
      e.HasOne(p => p.Department).WithMany(d => d.Programs).HasForeignKey(p => p.DepartmentId).OnDelete(DeleteBehavior.Restrict);
    });
 
    // Event
    modelBuilder.Entity<Event>(e => {
      e.HasKey(ev => ev.EventId);
      e.Property(ev => ev.EventType).HasConversion<string>();
      e.Property(ev => ev.Status).HasConversion<string>();
 
      // M:1 Event -> User (organizer)
      e.HasOne(ev => ev.Creator).WithMany(u => u.OrganizedEvents).HasForeignKey(ev => ev.CreatedBy).OnDelete(DeleteBehavior.Restrict);
    });
 
    // EventRSVP
    modelBuilder.Entity<EventRSVP>(e => {
      e.HasKey(r => r.RsvpId);
      e.HasIndex(r => new { r.EventId, r.AlumniId }).IsUnique(); // one RSVP per alumni per event
      e.Property(r => r.Status).HasConversion<string>();
 
      e.HasOne(r => r.Event).WithMany(ev => ev.RSVPs).HasForeignKey(r => r.EventId).OnDelete(DeleteBehavior.Cascade);
      e.HasOne(r => r.Alumni).WithMany(a => a.RSVPs).HasForeignKey(r => r.AlumniId).OnDelete(DeleteBehavior.Cascade);
    });
 
    // JobPosting
    modelBuilder.Entity<JobPosting>(e => {
      e.HasKey(j => j.JobId);
      e.Property(j => j.JobType).HasConversion<string>();
 
      e.HasOne(j => j.PostedByAlumni).WithMany(a => a.JobPostings).HasForeignKey(j => j.PostedBy).OnDelete(DeleteBehavior.Cascade);
    });
 
    // Donation
    modelBuilder.Entity<Donation>(e => {
      e.HasKey(d => d.DonationId);
      e.Property(d => d.Amount).HasPrecision(10, 2);
      e.Property(d => d.PaymentMethod).HasConversion<string>();
 
      e.HasOne(d => d.Alumni).WithMany(a => a.Donations).HasForeignKey(d => d.AlumniId).OnDelete(DeleteBehavior.Restrict);
    });
 
    // Newsletter
    modelBuilder.Entity<Newsletter>(e => {
      e.HasKey(n => n.NewsletterId);
      e.Property(n => n.Status).HasConversion<string>();
 
      e.HasOne(n => n.Creator).WithMany(u => u.Newsletters).HasForeignKey(n => n.CreatedBy).OnDelete(DeleteBehavior.Restrict);
    });
 
    // NewsletterRecipient
    modelBuilder.Entity<NewsletterRecipient>(e => {
      e.HasKey(r => r.RecipientId);
      e.HasIndex(r => new { r.NewsletterId, r.UserId }).IsUnique();
 
      e.HasOne(r => r.Newsletter).WithMany(n => n.Recipients).HasForeignKey(r => r.NewsletterId).OnDelete(DeleteBehavior.Cascade);
      e.HasOne(r => r.User).WithMany(u => u.NewsletterRecipients).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);
    });
 
    // Message
    modelBuilder.Entity<Message>(e => {
      e.HasKey(m => m.MessageId);
 
      // Two FK to same table — must name them explicitly
      e.HasOne(m => m.Sender).WithMany(u => u.SentMessages).HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.Restrict);
      e.HasOne(m => m.Receiver).WithMany(u => u.ReceivedMessages).HasForeignKey(m => m.ReceiverId).OnDelete(DeleteBehavior.Restrict);
    });
 
    // Survey
    modelBuilder.Entity<Survey>(e => {
      e.HasKey(s => s.SurveyId);
      e.Property(s => s.Status).HasConversion<string>();
 
      e.HasOne(s => s.Creator).WithMany(u => u.Surveys).HasForeignKey(s => s.CreatedBy).OnDelete(DeleteBehavior.Restrict);
    });
 
    // SurveyQuestion
    modelBuilder.Entity<SurveyQuestion>(e => {
      e.HasKey(q => q.QuestionId);
      e.Property(q => q.QuestionType).HasConversion<string>();
 
      e.HasOne(q => q.Survey).WithMany(s => s.Questions).HasForeignKey(q => q.SurveyId).OnDelete(DeleteBehavior.Cascade);
    });
 
    // SurveyResponse
    modelBuilder.Entity<SurveyResponse>(e => {
      e.HasKey(r => r.ResponseId);
      e.HasIndex(r => new { r.SurveyId, r.AlumniId }).IsUnique(); // one response per alumni
 
      e.HasOne(r => r.Survey).WithMany(s => s.Responses).HasForeignKey(r => r.SurveyId).OnDelete(DeleteBehavior.Cascade);
      e.HasOne(r => r.Alumni).WithMany(a => a.Responses).HasForeignKey(r => r.AlumniId).OnDelete(DeleteBehavior.Cascade);
    });
 
    // SurveyAnswer
    modelBuilder.Entity<SurveyAnswer>(e => {
      e.HasKey(a => a.AnswerId);
 
      e.HasOne(a => a.Response).WithMany(r => r.Answers).HasForeignKey(a => a.ResponseId).OnDelete(DeleteBehavior.Cascade);
      e.HasOne(a => a.Question).WithMany(q => q.Answers).HasForeignKey(a => a.QuestionId).OnDelete(DeleteBehavior.Restrict);
    });
 
    // Contact
    modelBuilder.Entity<Contact>(e => {
      e.HasKey(c => c.ContactId);
      e.Property(c => c.Type).HasConversion<string>();
 
      e.HasOne(c => c.Alumni).WithMany(a => a.Contacts).HasForeignKey(c => c.AlumniId).OnDelete(DeleteBehavior.Cascade);
    });
  }
}
