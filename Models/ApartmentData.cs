﻿namespace Apartments.Models
{
    public class ApartmentData
    {
        public IEnumerable<Apartment> Apartments { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<ApartmentCategory> ApartmentCategories { get; set; }
    }
}
