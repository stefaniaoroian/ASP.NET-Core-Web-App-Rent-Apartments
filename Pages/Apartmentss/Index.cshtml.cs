using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Apartments.Data;
using Apartments.Models;

namespace Apartments.Pages.Apartmentss
{
    public class IndexModel : PageModel
    {
        private readonly Apartments.Data.ApartmentsContext _context;

        public IndexModel(Apartments.Data.ApartmentsContext context)
        {
            _context = context;
        }

        public IList<Apartment> Apartment { get; set; } = default!;
        public ApartmentData ApartmentD { get; set; }
        public int ApartmentID { get; set; }
        public int CategoryID { get; set; }
        public string TitleSort { get; set; }
        public string PriceSort { get; set; }
        public string CurrentFilter { get; set; }
        public async Task OnGetAsync(int? id, int? categoryID, string sortOrder, string searchString)
        {
            ApartmentD = new ApartmentData();

            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            PriceSort = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";

            CurrentFilter = searchString;

            ApartmentD.Apartments = await _context.Apartment
            .Include(b => b.Agent)
            .Include(b => b.Owner)
            .Include(b => b.ApartmentCategories)
            .ThenInclude(b => b.Category)
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                ApartmentD.Apartments = ApartmentD.Apartments.Where(s => s.Owner.FirstName.Contains(searchString)
               || s.Owner.LastName.Contains(searchString)
               || s.Title.Contains(searchString));
            }

            if (id != null)
            {
                ApartmentID = id.Value;
                Apartment apartment = ApartmentD.Apartments
                .Where(i => i.ID == id.Value).Single();
                ApartmentD.Categories = apartment.ApartmentCategories.Select(s => s.Category);
            }

            switch (sortOrder)
            {
                case "title_desc":
                    ApartmentD.Apartments = ApartmentD.Apartments.OrderByDescending(s =>
                   s.Title);
                    break;
                case "price_desc":
                    ApartmentD.Apartments = ApartmentD.Apartments.OrderByDescending(s =>
                   s.Price);
                    break;


            }
        }
    }
}
