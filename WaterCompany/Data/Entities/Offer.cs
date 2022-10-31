namespace WaterCompany.Data.Entities
{
    public class Offer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Echelon { get; set; }

        public bool IsAvailable { get; set; }

        public User User { get; set; }
    }
}
