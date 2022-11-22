using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrganicFertilizerRecomondation.Models
{
    [Table("CropTypes")]
    public class CropType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
