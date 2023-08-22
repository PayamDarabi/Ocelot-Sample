namespace Article.Api.Repository.Interfaces
{
    public interface IArticleRepository
    {
        Task<int> DeleteAsync(int id);
        Task<Models.Article?> GetAsync(int id);
        Task<List<Models.Article>> GetAllAsync();
    }
}
