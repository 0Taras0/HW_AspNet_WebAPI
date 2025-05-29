﻿using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file);
        Task<string> SaveImageFromUrlAsync(string imageUrl);
        Task DeleteImageAsync(string name);
        Task<string> SaveImageFromBase64Async(string name);
    }
}
