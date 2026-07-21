using System;

namespace CheckZone.Api.DTOs
{
    public class BlogArticleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = "Cảnh báo phổ thông";
        public DateTime CreatedAt { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Status { get; set; } = "Đã đăng";
        public string? Content { get; set; }
        public string Thumbnail { get; set; } = string.Empty;
    }
}
