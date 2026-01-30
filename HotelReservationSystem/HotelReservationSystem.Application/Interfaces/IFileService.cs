using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HotelReservationSystem.Application.Interfaces;

public interface IFileService
{
    Task<string> SaveImageFileAsync(IFormFile imageFile, string folderName);
}
