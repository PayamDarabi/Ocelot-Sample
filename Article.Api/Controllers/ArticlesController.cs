using Article.Api.Models;
using Article.Api.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Redis.Cache.Cache.Interface;

namespace Article.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICacheService _cacheService;

        public ArticlesController(IArticleRepository articleRepository, ICacheService cacheService)
        {
            _articleRepository = articleRepository;
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cacheData = await _cacheService.GetData<List<Models.Article>>("Articles");
            if (cacheData is not null)
            {
                return Ok(cacheData);
            }
            cacheData = await _articleRepository.GetAllAsync();
            if (cacheData is not null)
            {
                _cacheService.SetData<IEnumerable<Models.Article>>("Articles", cacheData, DateTimeOffset.Now.AddSeconds(30));
                return Ok(cacheData);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cachedArticle = await _cacheService.GetData<Models.Article>("Article-" + id);
            if (cachedArticle is not null)
            {
                return Ok(cachedArticle);
            }
            cachedArticle = await _articleRepository.GetAsync(id);
            if (cachedArticle is not null)
            {
                _cacheService.SetData("Article-" + id, cachedArticle, DateTimeOffset.Now.AddSeconds(30));
                return Ok(cachedArticle);
            }

            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedId = await _articleRepository.DeleteAsync(id);

            if (deletedId == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}