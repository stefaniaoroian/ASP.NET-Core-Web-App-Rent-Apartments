namespace Apartments.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public ICollection<ApartmentCategory>? ApartmentCategories { get; set; }
        public IEnumerable<Apartment> Apartments { get; internal set; }
    }
}
