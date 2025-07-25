﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Model.Category
{
    public class CategoryUpdateModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; } = null;
    }
}
