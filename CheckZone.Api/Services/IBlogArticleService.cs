using System.Collections.Generic;
using System.Threading.Tasks;
using CheckZone.Api.DTOs;

namespace CheckZone.Api.Services
{
    public interface IBlogArticleService
    {
        Task<IEnumerable<BlogArticleDto>> GetAllPublishedAsync();
        Task<IEnumerable<BlogArticleDto>> GetAllAsync();
        Task<BlogArticleDto?> GetByIdOrSlugAsync(string idOrSlug);
        Task<BlogArticleDto> CreateAsync(CreateBlogArticleDto dto);
        Task<bool> UpdateAsync(string id, UpdateBlogArticleDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
