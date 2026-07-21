using System.ComponentModel.DataAnnotations;

namespace CheckZone.Api.DTOs
{
    public class UpdateBlogArticleDto
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Category { get; set; } = "Cảnh báo phổ thông";

        [StringLength(255)]
        public string? Slug { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Đã đăng";

        public string? Content { get; set; }

        [StringLength(2048)]
        public string Thumbnail { get; set; } = string.Empty;
    }
}
