using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CheckZone.Api.DTOs;
using CheckZone.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace CheckZone.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class BlogArticlesController : ControllerBase
    {
        private readonly IBlogArticleService _blogArticleService;

        public BlogArticlesController(IBlogArticleService blogArticleService)
        {
            _blogArticleService = blogArticleService;
        }

        [HttpGet("public/blogs")]
        public async Task<ActionResult<IEnumerable<BlogArticleDto>>> GetPublicBlogs()
        {
            var result = await _blogArticleService.GetAllPublishedAsync();
            return Ok(result);
        }

        [HttpGet("public/blogs/{idOrSlug}")]
        public async Task<ActionResult<BlogArticleDto>> GetBlogByIdOrSlug(string idOrSlug)
        {
            var result = await _blogArticleService.GetByIdOrSlugAsync(idOrSlug);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("admin/blogs")]
        public async Task<ActionResult<IEnumerable<BlogArticleDto>>> GetAdminBlogs()
        {
            var result = await _blogArticleService.GetAllAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpPost("admin/blogs")]
        public async Task<ActionResult<BlogArticleDto>> CreateBlog([FromBody] CreateBlogArticleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _blogArticleService.CreateAsync(dto);
            return Created($"/api/public/blogs/{result.Id}", result);
        }

        [Authorize]
        [HttpPut("admin/blogs/{id}")]
        public async Task<IActionResult> UpdateBlog(string id, [FromBody] UpdateBlogArticleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _blogArticleService.UpdateAsync(id, dto);
            if (!success)
            {
                return NotFound();
            }
            return Ok();
        }

        [Authorize]
        [HttpDelete("admin/blogs/{id}")]
        public async Task<IActionResult> DeleteBlog(string id)
        {
            var success = await _blogArticleService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
