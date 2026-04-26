using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Models;

namespace VMS.Services.Interfaces
{
    public interface ICameraService
    {
        Task<List<CameraModel>> GetCamerasAsync();
        Task<CameraModel?> CreateAsync(CameraCreateDto dto);
        Task<CameraModel?> UpdateAsync(uint id, CameraUpdateDto dto);
        Task<bool> DeleteAsync(uint id);
    }
}
