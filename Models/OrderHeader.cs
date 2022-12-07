namespace FinalProject.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }    
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string ZipCode { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public int AlbumCount { get; set; }

    }
}
