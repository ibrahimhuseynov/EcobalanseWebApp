using System.Collections.Generic;

namespace Lacromis.Models
{
    public class Catagory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Garbage> Garbages { get; set; }
    }
}
