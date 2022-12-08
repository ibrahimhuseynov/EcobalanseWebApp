using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lacromis.Models
{
    public class Garbage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string ImgUrl { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public double Price { get; set; }

        public string Description { get; set; }
        public string Descrition2 { get; set; }
        public string Descrition3 { get; set; }
        public int CatagoryId { get; set; }
        public Catagory Catagory { get; set; }
    }
}
