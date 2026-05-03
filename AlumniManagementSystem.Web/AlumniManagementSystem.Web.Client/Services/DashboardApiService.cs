namespace AlumniManagementSystem.Web.Client.Services;

public class DashboardApiService{
  private readonly HttpClient _http;
  public DashboardApiService(HttpClient http) => _http= http;

  public async Task<DashboardStatsDto?> GetStatsAsync() => await _http.GetFromJsonAsync<DashboardStatsDto>("api/dashboard/stats");

  public async Task<List<YearCountDto>> GetAlumniByYearAsync() => await _http.GetFromJsonAsync<List<YearCountDto>>("api/dashboard/alumni-by-year") ?? [];

  public async Task<List<DeptCountDto>> GetAlumniByDeptAsync() => await _http.GetFromJsonAsync<List<DeptCountDto>>("api/dashboard/alumni-by-department") ?? [];
}

// Local DTOs for dashboard responses 
public class DashboardStatsDto{
  public int TotalAlumni { get; set; }
  public int ProfilesComplete { get; set; }
  public int UpcomingEvents { get; set; }
  public int ActiveJobs { get; set; }
  public decimal TotalDonations { get; set; }
  public int UnreadMessages { get; set; }
  public int TotalMessages { get; set; }
  public int ActiveSurveys { get; set; }
  public int TotalDepartments { get; set; }
  public int ActivePrograms { get; set; }
}
public class YearCountDto{ public int? Year { get; set; } public int Count { get; set; } }
public class DeptCountDto{ public string Department { get; set; }= ""; public int Count { get; set; } }
