using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrganicFertilizerRecomondation.Models
{
    [Table("NaturalSources")]
    public class NaturalSource
    {
        [Key]
        public int Id { get; set; }
        public string Source { get; set; }
        public double Nitrigion { get; set; }
        public double Phosphurus { get; set; }
        public double Pottasium { get; set; }
    }
}
