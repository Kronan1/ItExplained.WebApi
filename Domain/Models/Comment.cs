namespace it_explained.WebApi.Domain.Models
{
    public class Comment
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? MovieId { get; set; }
        public string? Text { get; set; }
        public DateTime Date { get; set; }
    }
    
}