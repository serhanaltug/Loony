using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.Product
{
    public class CareInstruction
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
    }
}
