using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Apartments.Data;
using Apartments.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Apartments.Pages.Apartmentss
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : ApartmentCategoriesPageModel
    {

        private readonly Apartments.Data.ApartmentsContext _context;

        public CreateModel(Apartments.Data.ApartmentsContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        { 
            ViewData["OwnerID"] = new SelectList(_context.Set<Models.Owner>(), "ID", "FullName");
            ViewData["AgentID"] = new SelectList(_context.Set<Models.Agent>(), "ID", "AgentName");
            var apartment = new Apartment();
            apartment.ApartmentCategories = new List<ApartmentCategory>();
            PopulateAssignedCategoryData(_context, apartment);
            return Page();
        }

        [BindProperty]
        public Apartment Apartment { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
            var newApartment = new Apartment();
            if (selectedCategories != null)
            {
                newApartment.ApartmentCategories = new List<ApartmentCategory>();
                foreach (var cat in selectedCategories)
                {
                    var catToAdd = new ApartmentCategory
                    {
                        CategoryID = int.Parse(cat)
                    };
                    newApartment.ApartmentCategories.Add(catToAdd);
                }
            }
            if (await TryUpdateModelAsync<Apartment>(
            newApartment,
            "Apartment",
            i => i.Title, i => i.Owner,
            i => i.Price, i => i.Date, i => i.AgentID))
            {
                _context.Apartment.Add(newApartment);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            PopulateAssignedCategoryData(_context, newApartment);
            return Page();
        }

    }
}
