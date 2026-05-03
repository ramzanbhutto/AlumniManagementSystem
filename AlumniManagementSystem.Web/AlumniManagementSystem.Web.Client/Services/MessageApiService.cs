namespace AlumniManagementSystem.Web.Client.Services;

public class MessageApiService{
  private readonly HttpClient _http;
  public MessageApiService(HttpClient http) => _http= http;

  public async Task<List<MessageDto>> GetInboxAsync() => await _http.GetFromJsonAsync<List<MessageDto>>("api/messages/inbox") ?? [];

  public async Task<List<MessageDto>> GetSentAsync() => await _http.GetFromJsonAsync<List<MessageDto>>("api/messages/sent") ?? [];

  public async Task<(bool ok, string? err)> SendAsync(SendMessageDto dto){
    var r= await _http.PostAsJsonAsync("api/messages", dto);
    return r.IsSuccessStatusCode ? (true, null) : (false, await r.Content.ReadAsStringAsync());
  }

  public async Task<RollLookupDto?> GetUserIdByRollAsync(string roll){
    try{
        return await _http.GetFromJsonAsync<RollLookupDto>($"api/users/by-roll/{Uri.EscapeDataString(roll)}");
    }
    catch{ return null; }
  } 

  public async Task MarkAsReadAsync(Guid messageId){
    try{ await _http.PatchAsync($"api/messages/{messageId}/read", null); }
    catch{ }
  }
}
