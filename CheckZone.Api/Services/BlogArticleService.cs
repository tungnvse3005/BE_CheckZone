using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CheckZone.Api.Data;
using CheckZone.Api.DTOs;
using CheckZone.Api.Entities;

namespace CheckZone.Api.Services
{
    public class BlogArticleService : IBlogArticleService
    {
        private readonly AppDbContext _context;

        public BlogArticleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlogArticleDto>> GetAllPublishedAsync()
        {
            var articles = await _context.BlogArticles
                .Where(x => x.Status == "Đã đăng")
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return articles.Select(MapToDto);
        }

        public async Task<IEnumerable<BlogArticleDto>> GetAllAsync()
        {
            var articles = await _context.BlogArticles
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return articles.Select(MapToDto);
        }

        public async Task<BlogArticleDto?> GetByIdOrSlugAsync(string idOrSlug)
        {
            var article = await _context.BlogArticles
                .FirstOrDefaultAsync(x => x.Id == idOrSlug || x.Slug == idOrSlug);

            return article == null ? null : MapToDto(article);
        }

        public async Task<BlogArticleDto> CreateAsync(CreateBlogArticleDto dto)
        {
            var random = new Random();
            string generatedId;
            bool exists;
            do
            {
                generatedId = $"ART-{random.Next(1000, 9999)}";
                exists = await _context.BlogArticles.AnyAsync(x => x.Id == generatedId);
            } while (exists);

            var slug = string.IsNullOrWhiteSpace(dto.Slug)
                ? GenerateSlug(dto.Title)
                : dto.Slug.Trim();

            var thumb = !string.IsNullOrWhiteSpace(dto.Thumbnail) ? dto.Thumbnail.Trim() : string.Empty;

            var article = new BlogArticle
            {
                Id = generatedId,
                Title = dto.Title.Trim(),
                Category = dto.Category ?? "Cảnh báo phổ thông",
                CreatedAt = DateTime.UtcNow,
                Slug = slug,
                Status = dto.Status ?? "Đã đăng",
                Content = dto.Content,
                Thumbnail = thumb,
                ThumbnailUrl = thumb
            };

            _context.BlogArticles.Add(article);
            await _context.SaveChangesAsync();

            return MapToDto(article);
        }

        public async Task<bool> UpdateAsync(string id, UpdateBlogArticleDto dto)
        {
            var article = await _context.BlogArticles.FindAsync(id);
            if (article == null) return false;

            article.Title = dto.Title.Trim();
            article.Category = dto.Category ?? "Cảnh báo phổ thông";
            article.Slug = string.IsNullOrWhiteSpace(dto.Slug) ? GenerateSlug(dto.Title) : dto.Slug.Trim();
            article.Status = dto.Status ?? "Đã đăng";
            article.Content = dto.Content;
            
            var thumb = !string.IsNullOrWhiteSpace(dto.Thumbnail) ? dto.Thumbnail.Trim() : string.Empty;
            article.Thumbnail = thumb;
            article.ThumbnailUrl = thumb;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var article = await _context.BlogArticles.FindAsync(id);
            if (article == null) return false;

            _context.BlogArticles.Remove(article);
            await _context.SaveChangesAsync();
            return true;
        }

        private static BlogArticleDto MapToDto(BlogArticle article)
        {
            var thumb = !string.IsNullOrWhiteSpace(article.Thumbnail)
                ? article.Thumbnail
                : (!string.IsNullOrWhiteSpace(article.ThumbnailUrl) ? article.ThumbnailUrl : string.Empty);

            return new BlogArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Category = article.Category,
                CreatedAt = article.CreatedAt,
                Slug = article.Slug,
                Status = article.Status,
                Content = article.Content,
                Thumbnail = thumb
            };
        }

        private static string GenerateSlug(string phrase)
        {
            string str = phrase.ToLower().Trim();
            str = Regex.Replace(str, @"[àáạảãâầấậẩẫăằắặẳẵ]", "a");
            str = Regex.Replace(str, @"[èéẹẻẽêềếệểễ]", "e");
            str = Regex.Replace(str, @"[ìíịỉĩ]", "i");
            str = Regex.Replace(str, @"[òóọỏõôồốộổỗơờớợởỡ]", "o");
            str = Regex.Replace(str, @"[ùúụủũưừứựửữ]", "u");
            str = Regex.Replace(str, @"[ỳýỵỷỹ]", "y");
            str = Regex.Replace(str, @"[đ]", "d");
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = Regex.Replace(str, @"\s", "-");
            return str;
        }
    }
}
