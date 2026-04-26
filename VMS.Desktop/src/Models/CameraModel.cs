using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VMS.Models
{
    public enum CameraStatus { Unknown, Online, Offline }

    public class CameraModel : INotifyPropertyChanged
    {
        private CameraStatus _status = CameraStatus.Unknown;
        private bool _isRecording;

        public uint   Id            { get; set; }
        public string Name          { get; set; } = string.Empty;
        public uint   VRServerId    { get; set; }
        public string Protocol      { get; set; } = "rtsp";
        public string RtspUrl       { get; set; } = string.Empty;
        public string Username      { get; set; } = string.Empty;
        public string Location      { get; set; } = string.Empty;
        public bool   PtzSupported  { get; set; }
        public string OnvifHost     { get; set; } = string.Empty;
        public uint   OnvifPort     { get; set; }
        public string OnvifUsername { get; set; } = string.Empty;

        public CameraStatus Status
        {
            get => _status;
            set
            {
                if (_status == value) return;
                _status = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsOnline));
                OnPropertyChanged(nameof(StatusText));
            }
        }

        public bool IsRecording
        {
            get => _isRecording;
            set
            {
                if (_isRecording == value) return;
                _isRecording = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RecordingText));
            }
        }

        public bool   IsOnline      => Status == CameraStatus.Online;
        public string StatusText    => Status.ToString();
        public string RecordingText => IsRecording ? "Stop" : "Record";
        public string ProtocolBadge => Protocol.ToUpperInvariant();

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
