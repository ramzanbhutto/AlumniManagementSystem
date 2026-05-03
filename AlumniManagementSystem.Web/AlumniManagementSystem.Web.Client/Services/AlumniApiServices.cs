namespace AlumniManagementSystem.Web.Client.Services;

public class AlumniApiService{
  private readonly HttpClient _http;
  public AlumniApiService(HttpClient http) => _http= http;

  public async Task<List<AlumniDto>> GetAllAsync()  => await _http.GetFromJsonAsync<List<AlumniDto>>("api/alumni") ?? [];

  public async Task<AlumniDto?> GetByRollAsync(string roll)  => await _http.GetFromJsonAsync<AlumniDto>($"api/alumni/{roll}");

  public async Task<(bool ok, string? err)> CreateAsync(CreateAlumniDto dto){
    var r= await _http.PostAsJsonAsync("api/alumni", dto);
    return r.IsSuccessStatusCode ? (true, null) : (false, await r.Content.ReadAsStringAsync());
  }

  public async Task<(bool ok, string? err)> UpdateAsync(Guid id, CreateAlumniDto dto){
    var r= await _http.PutAsJsonAsync($"api/alumni/{id}", dto);
    return r.IsSuccessStatusCode ? (true, null) : (false, await r.Content.ReadAsStringAsync());
  }

  public async Task<bool> DeleteAsync(Guid id)  => (await _http.DeleteAsync($"api/alumni/{id}")).IsSuccessStatusCode;

  public async Task<object?> SearchAsync(string? name, string? dept, int? gradYear, string? city, string? company){
    var q= $"api/search/alumni?page=1&pageSize=50";
    if(!string.IsNullOrEmpty(name)) q+= $"&name={Uri.EscapeDataString(name)}";
    if(!string.IsNullOrEmpty(dept)) q+= $"&dept={dept}";
    if(gradYear.HasValue) q+= $"&gradYear={gradYear}";
    if(!string.IsNullOrEmpty(city)) q+= $"&city={Uri.EscapeDataString(city)}";
    if(!string.IsNullOrEmpty(company)) q+= $"&company={Uri.EscapeDataString(company)}";
    return await _http.GetFromJsonAsync<object>(q);
  }

  public async Task<(bool ok, string? err)> RegisterAlumniAsync(RegisterDto dto){
    var r = await _http.PostAsJsonAsync("api/auth/register", dto);
    return r.IsSuccessStatusCode ? (true, null) : (false, await r.Content.ReadAsStringAsync());
  }

  public async Task<List<ProgramDto>> GetProgramsAsync() => await _http.GetFromJsonAsync<List<ProgramDto>>("api/programs") ?? [];

  public async Task<(bool ok, string? err)> AddProgramAsync(Guid departmentId, CreateProgramDto dto){
    var r = await _http.PostAsJsonAsync($"api/departments/{departmentId}/programs", dto);
    return r.IsSuccessStatusCode ? (true, null) : (false, await r.Content.ReadAsStringAsync());
}

  public async Task<List<DepartmentDto>> GetDepartmentsAsync() => await _http.GetFromJsonAsync<List<DepartmentDto>>("api/departments") ?? [];

  public async Task<AlumniDto?> GetByIdAsync(Guid id) => await _http.GetFromJsonAsync<AlumniDto>($"api/alumni/id/{id}");

  public async Task<(bool ok, string? err)> UpdateProfileAsync(Guid id, UpdateAlumniDto dto){
    var r = await _http.PutAsJsonAsync($"api/alumni/{id}/profile", dto);
    return r.IsSuccessStatusCode ? (true, null) : (false, await r.Content.ReadAsStringAsync());
  }
}
