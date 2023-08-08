namespace Article.Api.Repository.Interfaces
{
    public interface IArticleRepository
    {
        int Delete(int id);
        Models.Article? Get(int id);
        List<Models.Article> GetAll();
    }
}
