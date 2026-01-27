using Microsoft.AspNetCore.Http;

namespace HotelReservationSystem.Web.Services
{
    public interface IFileService
    {
        Task<string> SaveImageFileAsync(IFormFile imageFile, string folderName);
    }
}