using System.Collections.Generic;
using VMS.Models;
using VMS.Services;
using VMS.ViewModels.Base;

namespace VMS.ViewModels.Pages
{
    public class AddEditCameraViewModel : ViewModelBase
    {
        private string _protocol = "rtsp";

        public string Title     { get; }
        public uint?  EditId    { get; }

        public string Name          { get; set; } = string.Empty;
        public uint   VRServerId    { get; set; } = 1;
        public string Location      { get; set; } = string.Empty;

        public string Protocol
        {
            get => _protocol;
            set
            {
                if (SetProperty(ref _protocol, value))
                {
                    OnPropertyChanged(nameof(IsRtsp));
                    OnPropertyChanged(nameof(IsOnvif));
                }
            }
        }

        // RTSP fields
        public string RtspUrl  { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // ONVIF fields
        public string OnvifHost     { get; set; } = string.Empty;
        public int    OnvifPort     { get; set; } = 80;
        public string OnvifUsername { get; set; } = string.Empty;
        public string OnvifPassword { get; set; } = string.Empty;

        public bool IsRtsp  => Protocol == "rtsp";
        public bool IsOnvif => Protocol == "onvif";

        public List<string> Protocols { get; } = new() { "rtsp", "onvif" };

        public AddEditCameraViewModel()
        {
            Title = "Add Camera";
        }

        public AddEditCameraViewModel(CameraModel camera)
        {
            Title       = "Edit Camera";
            EditId      = camera.Id;
            Name        = camera.Name;
            VRServerId  = camera.VRServerId;
            Protocol    = camera.Protocol;
            Location    = camera.Location;
            RtspUrl     = camera.RtspUrl;
            Username    = camera.Username;
            OnvifHost   = camera.OnvifHost;
            OnvifPort   = (int)camera.OnvifPort;
            OnvifUsername = camera.OnvifUsername;
            // passwords not pre-filled for security
        }

        public CameraCreateDto ToCreateDto() => new()
        {
            Name         = Name.Trim(),
            VRServerId   = VRServerId,
            Protocol     = Protocol,
            RtspUrl      = RtspUrl.Trim(),
            Username     = Username.Trim(),
            Password     = Password,
            OnvifHost    = OnvifHost.Trim(),
            OnvifPort    = OnvifPort,
            OnvifUsername = OnvifUsername.Trim(),
            OnvifPassword = OnvifPassword,
            Location     = Location.Trim(),
        };

        public CameraUpdateDto ToUpdateDto() => new()
        {
            Name         = Name.Trim(),
            Location     = Location.Trim(),
            RtspUrl      = RtspUrl.Trim(),
            Username     = Username.Trim(),
            Password     = string.IsNullOrEmpty(Password) ? null : Password,
            OnvifHost    = OnvifHost.Trim(),
            OnvifPort    = OnvifPort,
            OnvifUsername = OnvifUsername.Trim(),
            OnvifPassword = string.IsNullOrEmpty(OnvifPassword) ? null : OnvifPassword,
        };

        public string? Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))           return "Name is required.";
            if (VRServerId == 0)                           return "VR Server ID must be greater than 0.";
            if (Protocol == "rtsp"  && string.IsNullOrWhiteSpace(RtspUrl))  return "RTSP URL is required.";
            if (Protocol == "onvif" && string.IsNullOrWhiteSpace(OnvifHost)) return "ONVIF host is required.";
            if (Protocol == "onvif" && OnvifPort <= 0)    return "ONVIF port must be greater than 0.";
            return null;
        }
    }
}
