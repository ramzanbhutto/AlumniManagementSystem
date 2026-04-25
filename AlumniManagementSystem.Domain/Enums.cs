namespace AlumniManagementSystem.Domain.Enums;

public enum UserRole { Admin, Alumni, Guest }
public enum Gender { Male, Female, Other }
public enum EventType { Reunion, Seminar, CareerFair, Social, Workshop }
public enum EventStatus { Upcoming, Ongoing, Completed, Cancelled }
public enum RsvpStatus { Attending, Declined, Waitlisted }
public enum JobType { FullTime, PartTime, Remote, Contract }
public enum QuestionType { MCQ, Text, Rating }
public enum SurveyStatus { Active, Closed }
public enum NewsletterStatus { Draft, Sent }
public enum ContactType { Phone, Email, LinkedIn, Twitter, Other }
public enum PaymentMethod { Card, BankTransfer, JazzCash, EasyPaisa, Other }
