using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VMS.Infrastructure;
using VMS.Models;
using VMS.Services.Interfaces;

namespace VMS.Services
{
    // DTOs matching the Go control-server JSON (PascalCase, no json tags on model)
    internal class CameraDto
    {
        [JsonPropertyName("ID")]          public uint   Id            { get; set; }
        [JsonPropertyName("Name")]        public string Name          { get; set; } = "";
        [JsonPropertyName("VRServerID")]  public uint   VRServerId    { get; set; }
        [JsonPropertyName("Protocol")]    public string Protocol      { get; set; } = "rtsp";
        [JsonPropertyName("RTSPURL")]     public string RtspUrl       { get; set; } = "";
        [JsonPropertyName("Username")]    public string Username      { get; set; } = "";
        [JsonPropertyName("Recording")]   public bool   Recording     { get; set; }
        [JsonPropertyName("Status")]      public string Status        { get; set; } = "";
        [JsonPropertyName("Location")]    public string Location      { get; set; } = "";
        [JsonPropertyName("OnvifHost")]   public string OnvifHost     { get; set; } = "";
        [JsonPropertyName("OnvifPort")]   public uint   OnvifPort     { get; set; }
        [JsonPropertyName("OnvifUsername")] public string OnvifUsername { get; set; } = "";
        [JsonPropertyName("PTZSupported")] public bool  PtzSupported  { get; set; }
    }

    public class CameraCreateDto
    {
        [JsonPropertyName("name")]       public string Name       { get; set; } = "";
        [JsonPropertyName("vrServerID")] public uint   VRServerId { get; set; }
        [JsonPropertyName("protocol")]   public string Protocol   { get; set; } = "rtsp";
        [JsonPropertyName("rtspUrl")]    public string RtspUrl    { get; set; } = "";
        [JsonPropertyName("username")]   public string Username   { get; set; } = "";
        [JsonPropertyName("password")]   public string Password   { get; set; } = "";
        [JsonPropertyName("onvifHost")]  public string OnvifHost  { get; set; } = "";
        [JsonPropertyName("onvifPort")]  public int    OnvifPort  { get; set; }
        [JsonPropertyName("onvifUsername")] public string OnvifUsername { get; set; } = "";
        [JsonPropertyName("onvifPassword")] public string OnvifPassword { get; set; } = "";
        [JsonPropertyName("location")]   public string Location   { get; set; } = "";
    }

    public class CameraUpdateDto
    {
        [JsonPropertyName("name")]       public string? Name       { get; set; }
        [JsonPropertyName("location")]   public string? Location   { get; set; }
        [JsonPropertyName("rtspUrl")]    public string? RtspUrl    { get; set; }
        [JsonPropertyName("username")]   public string? Username   { get; set; }
        [JsonPropertyName("password")]   public string? Password   { get; set; }
        [JsonPropertyName("onvifHost")]  public string? OnvifHost  { get; set; }
        [JsonPropertyName("onvifPort")]  public int     OnvifPort  { get; set; }
        [JsonPropertyName("onvifUsername")] public string? OnvifUsername { get; set; }
        [JsonPropertyName("onvifPassword")] public string? OnvifPassword { get; set; }
    }

    public class CameraService : ICameraService
    {
        private readonly HttpClient _http = new();
        private readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

        private string BaseUrl => $"{AppSettings.Instance.ApiBaseUrl}/api/v1/cameras";

        private void SetAuth()
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.Auth.AccessToken ?? "");
        }

        public async Task<List<CameraModel>> GetCamerasAsync()
        {
            SetAuth();
            var resp = await _http.GetAsync(BaseUrl);
            resp.EnsureSuccessStatusCode();
            var dtos = await resp.Content.ReadFromJsonAsync<List<CameraDto>>(_json)
                       ?? new List<CameraDto>();
            return dtos.ConvertAll(Map);
        }

        public async Task<CameraModel?> CreateAsync(CameraCreateDto dto)
        {
            SetAuth();
            var content = new StringContent(JsonSerializer.Serialize(dto, _json), Encoding.UTF8, "application/json");
            var resp = await _http.PostAsync(BaseUrl, content);
            resp.EnsureSuccessStatusCode();
            var created = await resp.Content.ReadFromJsonAsync<CameraDto>(_json);
            return created is null ? null : Map(created);
        }

        public async Task<CameraModel?> UpdateAsync(uint id, CameraUpdateDto dto)
        {
            SetAuth();
            var content = new StringContent(JsonSerializer.Serialize(dto, _json), Encoding.UTF8, "application/json");
            var req = new HttpRequestMessage(HttpMethod.Patch, $"{BaseUrl}/{id}") { Content = content };
            var resp = await _http.SendAsync(req);
            resp.EnsureSuccessStatusCode();
            var updated = await resp.Content.ReadFromJsonAsync<CameraDto>(_json);
            return updated is null ? null : Map(updated);
        }

        public async Task<bool> DeleteAsync(uint id)
        {
            SetAuth();
            var resp = await _http.DeleteAsync($"{BaseUrl}/{id}");
            return resp.IsSuccessStatusCode;
        }

        private static CameraModel Map(CameraDto d) => new()
        {
            Id            = d.Id,
            Name          = d.Name,
            VRServerId    = d.VRServerId,
            Protocol      = d.Protocol,
            RtspUrl       = d.RtspUrl,
            Username      = d.Username,
            Location      = d.Location,
            OnvifHost     = d.OnvifHost,
            OnvifPort     = d.OnvifPort,
            OnvifUsername = d.OnvifUsername,
            PtzSupported  = d.PtzSupported,
            IsRecording   = d.Recording,
            Status        = d.Status?.ToLower() switch
            {
                "online"  => CameraStatus.Online,
                "offline" => CameraStatus.Offline,
                _         => CameraStatus.Unknown,
            },
        };
    }
}
