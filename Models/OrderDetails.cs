using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int? OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeader? OrderHeader { get; set; }
        public int? AlbumId { get; set; }
        [ForeignKey("AlbumId")]
        public OrderHeader? Album { get; set; }
        public string ServiceName { get; set; }
        public int Price { get; set; }


    }
}
