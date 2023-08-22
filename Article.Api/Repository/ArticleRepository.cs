using Article.Api.Repository.Interfaces;

namespace Article.Api.Repository
{
    public class ArticleRepository : List<Models.Article>, IArticleRepository
    {
        private readonly static List<Models.Article> _articles = Populate();
        private static List<Models.Article> Populate()
        {
            var result = new List<Models.Article>()
        {
            new Models.Article
            {
                Id = 1,
                Title = "First Article",
                WriterId = 1,
                LastUpdate = DateTime.Now
            },
            new Models.Article
            {
                Id = 2,
                Title = "Second title",
                WriterId = 2,
                LastUpdate = DateTime.Now
            },
            new Models.Article
            {
                Id = 3,
                Title = "Third title",
                WriterId = 3,
                LastUpdate = DateTime.Now
            }
        };
            return result;
        }
        public async Task<List<Models.Article>> GetAllAsync()
        {
            return await Task.FromResult(_articles);
        }
        public async Task<Models.Article?> GetAsync(int id)
        {
            return await Task.FromResult(_articles.FirstOrDefault(x => x.Id == id));
        }
        public async Task<int> DeleteAsync(int id)
        {
            var removed = await Task.FromResult(_articles.SingleOrDefault(x => x.Id == id));
            if (removed != null)
            {
                _articles.Remove(removed);
            }
            return removed?.Id ?? 0;
        }
    }
}