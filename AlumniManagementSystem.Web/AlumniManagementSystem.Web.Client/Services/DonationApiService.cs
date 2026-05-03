namespace AlumniManagementSystem.Web.Client.Services;

public class DonationApiService{
  private readonly HttpClient _http;
  public DonationApiService(HttpClient http) => _http= http;

  public async Task<List<DonationDto>> GetMyAsync() => await _http.GetFromJsonAsync<List<DonationDto>>("api/donations/my") ?? [];

  public async Task<List<DonationDto>> GetAllAsync() => await _http.GetFromJsonAsync<List<DonationDto>>("api/donations") ?? [];

  public async Task<(bool ok, string? err)> CreateAsync(CreateDonationDto dto){
    var r= await _http.PostAsJsonAsync("api/donations", dto);
    return r.IsSuccessStatusCode ? (true, null) : (false, await r.Content.ReadAsStringAsync());
  }
}
