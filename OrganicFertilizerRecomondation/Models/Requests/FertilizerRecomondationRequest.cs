namespace OrganicFertilizerRecomondation.Models.Requests
{
    public class FertilizerRecomondationRequest
    {
        public int CropTypeId { get; set; }
        public int CropTypeAgeId { get; set; }
        //set default land area as 1 
        public double Area { get; set; } = 1;
        //set default NPK content of soil as 0
        public double NContentOfSoil { get; set; } = 0;
        public double PContentOfSoil { get; set; } = 0;
        public double KContentOfSoil { get; set; } = 0;
        //set default N content of soil as 1.5 Kg (15000 Mg)
        public double NContentOfComposte { get; set; } = 15000;
        //set default P content of soil as 1 Kg (10000 Mg)
        public double PContentOfComposte { get; set; } = 10000;
        //set default K content of soil as 1.5 Kg (15000 Mg)
        public double KContentOfComposte { get; set; } = 15000;
        public int NaturalSourceId { get; set; } = 0;
    }

    public class NaturalSourceAmountRequest
    {
        public int NaturalSourceId { get; set; }
    }

    public class CalItem
    {
        public string Item { get; set; }
        public double Value { get; set; }
    }
}
