using AlumniManagementSystem.Domain;
 
namespace AlumniManagementSystem.Application.Interfaces;
 
public interface IAuthService{
  string HashPassword(string plainText);
  bool VerifyPassword(string plainText, string hash);
  string GenerateJwtToken(User user);
}
