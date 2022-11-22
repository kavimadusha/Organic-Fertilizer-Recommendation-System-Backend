using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrganicFertilizerRecomondation.Models
{
    [Table("CropAges")]
    public class CropAge
    {
        [Key]
        public int Id { get; set; }
        public int CropTypeId { get; set; }
        public virtual CropType CropType { get; set; }
        public string Age { get; set; }
        public double Nitrigion { get; set; }
        public double Phosphurus { get; set; }
        public double Pottasium { get; set; }
    }
}
