namespace OcelotApiGateway.Dtos
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime LastUpdate { get; set; }
        public int WriterId { get; set; }
        public WriterDto? Writer { get; set; }
    }
}
