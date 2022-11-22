using System.Collections.Generic;

namespace OrganicFertilizerRecomondation.DTOs
{
    public class DropDownDTO
    {
        public int Value { get; set; }
        public string Label { get; set; }
    }

    public class CompostAmountDTO
    {
        public double NValue { get; set; }
        public double PValue { get; set; }
        public double KValue { get; set; }
        public double Amount { get; set; }
        public List<NaturalSourceAmount> NaturalSourceAmounts { get; set; }
    }

    public class NaturalSourceAmount
    {
        public string Name { get; set; }
        public double Amount { get; set; }
    }
}
