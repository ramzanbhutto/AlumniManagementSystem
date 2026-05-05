namespace AlumniManagementSystem.Web.Client.Services;

public class EventApiService{
  private readonly HttpClient _http;
  public EventApiService(HttpClient http) => _http= http;

  public async Task<List<EventDto>> GetAllAsync()  => await _http.GetFromJsonAsync<List<EventDto>>("api/events") ?? [];

  public async Task<List<EventDto>> GetUpcomingAsync()  => await _http.GetFromJsonAsync<List<EventDto>>("api/events/upcoming") ?? [];

  public async Task<(bool ok, string? err)> CreateAsync(CreateEventDto dto){
    var r= await _http.PostAsJsonAsync("api/events", dto);
    return r.IsSuccessStatusCode ? (true, null) : (false, await r.Content.ReadAsStringAsync());
  }

  public async Task<(bool ok, string? err)> RsvpAsync(Guid eventId){
    var r = await _http.PostAsync($"api/events/{eventId}/rsvp", null);
    if(r.IsSuccessStatusCode) return (true, null);
    var body = await r.Content.ReadAsStringAsync();
    return (false, body.Contains("already") ? "You already RSVPed to this event." : "Failed to RSVP.");
  }

  public async Task<(bool, string?)> CancelAsync(Guid eventId){
    var res= await _http.PatchAsync($"api/events/{eventId}/cancel", null);
    if(res.IsSuccessStatusCode) return (true, null);
    var err= await res.Content.ReadAsStringAsync();
    return (false, err);
  }

}
