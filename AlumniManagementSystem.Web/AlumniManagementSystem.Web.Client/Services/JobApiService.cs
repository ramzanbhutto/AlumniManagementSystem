namespace AlumniManagementSystem.Web.Client.Services;

public class JobApiService{
  private readonly HttpClient _http;
  public JobApiService(HttpClient http) => _http= http;

  public async Task<List<JobPostingDto>> GetAllAsync() => await _http.GetFromJsonAsync<List<JobPostingDto>>("api/jobs") ?? [];

  public async Task<(bool ok, string? err)> CreateAsync(CreateJobPostingDto dto){
    var r= await _http.PostAsJsonAsync("api/jobs", dto);
    return r.IsSuccessStatusCode ? (true, null) : (false, await r.Content.ReadAsStringAsync());
  }
}
