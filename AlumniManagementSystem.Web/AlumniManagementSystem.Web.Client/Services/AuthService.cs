using System.Net.Http.Json;
using System.Text.Json;
using AlumniManagementSystem.Shared;
using Microsoft.JSInterop;

namespace AlumniManagementSystem.Web.Client.Services;

public class AuthService{
  private readonly HttpClient _http;
  private readonly IJSRuntime _js;
  private bool _initialized=false;

  public string? Token { get; private set; }
  public string? Role { get; private set; }
  public string? FullName { get; private set; }
  public Guid UserId { get; private set; }
  public Guid AlumniId { get; private set; }
  public bool IsLoggedIn => !string.IsNullOrEmpty(Token);
  public bool IsAdmin => Role == "Admin";
  public bool IsAlumni => Role == "Alumni";

  public event Action? OnAuthChanged;

  public AuthService(HttpClient http, IJSRuntime js){
    _http= http;
    _js= js;
  }

  public async Task InitAsync(){
    if(_initialized) return;
    _initialized=true;
    try{
      Token= await _js.InvokeAsync<string?>("localStorage.getItem", "ams_token");
      Role= await _js.InvokeAsync<string?>("localStorage.getItem", "ams_role");
      FullName= await _js.InvokeAsync<string?>("localStorage.getItem", "ams_name");
      var uid= await _js.InvokeAsync<string?>("localStorage.getItem", "ams_uid");
      var aid= await _js.InvokeAsync<string?>("localStorage.getItem", "ams_aid");
      if(!string.IsNullOrEmpty(uid) && Guid.TryParse(uid, out var g1)) UserId= g1;
      if(!string.IsNullOrEmpty(aid) && Guid.TryParse(aid, out var g2)) AlumniId= g2;
      if(!string.IsNullOrEmpty(Token)) SetAuthHeader();
    }
    catch{ }
  }

  public async Task<(bool success, string? error)> LoginAsync(LoginDto dto){
    try{
      var response= await _http.PostAsJsonAsync("api/auth/login", dto);
      if(!response.IsSuccessStatusCode){
        var err= await response.Content.ReadAsStringAsync();
        return (false, "Invalid credentials.");
      }
      var result= await response.Content.ReadFromJsonAsync<AuthResponseDto>();
      if(result==null) return (false, "Unexpected error.");
      await SaveSessionAsync(result);
      return (true,null);
    }
    catch(Exception ex){
      return (false,ex.Message);
    }
  }

  public async Task<(bool success, string? error)> RegisterAsync(RegisterDto dto){
    try{
      var response= await _http.PostAsJsonAsync("api/auth/register", dto);
      if(!response.IsSuccessStatusCode){
        var body= await response.Content.ReadAsStringAsync();
        if(body.Contains("Email already")) return (false, "Email already registered.");
        if(body.Contains("RollNumber") || body.Contains("Roll")) return (false, "Roll number already taken.");
        if(body.Contains("program") || body.Contains("Program")) return (false, "No programs available. Contact admin.");
        return (false, "Registration failed. Please check all fields and try again.");
      }
      var result= await response.Content.ReadFromJsonAsync<AuthResponseDto>();
      if(result==null) return (false, "Unexpected error.");
      await SaveSessionAsync(result);
      return (true,null);
    }
    catch(Exception ex){
      return (false,ex.Message);
    }
  }

  public async Task LogoutAsync(){
    Token= null; Role = null; FullName = null;
    UserId= Guid.Empty; AlumniId = Guid.Empty;
    _initialized= false;
    _http.DefaultRequestHeaders.Authorization = null;
    try{
      await _js.InvokeVoidAsync("clearAuthStorage");
    }
    catch{}
      OnAuthChanged?.Invoke();
  }

  private async Task SaveSessionAsync(AuthResponseDto result){
    Token= result.Token;
    Role= result.Role;
    FullName= result.FullName;
    UserId= result.UserId;
    AlumniId= result.AlumniId;
    _initialized=true;
    SetAuthHeader();
    try{
         await _js.InvokeVoidAsync("localStorage.setItem", "ams_token", Token);
         await _js.InvokeVoidAsync("localStorage.setItem", "ams_role", Role);
         await _js.InvokeVoidAsync("localStorage.setItem", "ams_name", FullName);
         await _js.InvokeVoidAsync("localStorage.setItem", "ams_uid", UserId.ToString());
         await _js.InvokeVoidAsync("localStorage.setItem", "ams_aid", AlumniId.ToString());
      }
    catch{ }
    OnAuthChanged?.Invoke();
  }

  private void SetAuthHeader(){
    _http.DefaultRequestHeaders.Authorization= new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
  }
}
