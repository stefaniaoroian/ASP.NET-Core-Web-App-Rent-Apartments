using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Apartments.Data;
using Apartments.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Apartments.Pages.Apartmentss
{
    [Authorize(Roles = "Admin")]

    public class EditModel : ApartmentCategoriesPageModel
    {
        private readonly Apartments.Data.ApartmentsContext _context;

        public EditModel(Apartments.Data.ApartmentsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Apartment Apartment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Apartment == null)
            {
                return NotFound();
            }
           
            var apartment =  await _context.Apartment.FirstOrDefaultAsync(m => m.ID == id);

            if (apartment == null)
            {
                return NotFound();
            }

            Apartment = apartment;

            Apartment = await _context.Apartment
           .Include(b => b.Agent)
           .Include(b => b.ApartmentCategories).ThenInclude(b => b.Category)
           .AsNoTracking()
           .FirstOrDefaultAsync(m => m.ID == id);
            PopulateAssignedCategoryData(_context, Apartment);
         
            var ownerList = _context.Owner.Select(x => new
            {
                x.ID,
                FullName = x.FirstName + " " + x.LastName
            });
            ViewData["OwnerID"] = new SelectList(ownerList, "ID", "FullName");
            ViewData["AgentID"] = new SelectList(_context.Set<Models.Agent>(), "ID", "AgentName");
         
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }
            var apartmentToUpdate = await _context.Apartment
            .Include(i => i.Agent)
            .Include(i => i.ApartmentCategories)
            .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(s => s.ID == id);
            if (apartmentToUpdate == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync<Apartment>(
            apartmentToUpdate,
            "Apartment",
            i => i.Title, i => i.Owner,
            i => i.Price, i => i.Date, i => i.Agent))
            {
                UpdateApartmentCategories(_context, selectedCategories, apartmentToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            //Apelam UpdateBookCategories pentru a aplica informatiile din checkboxuri la entitatea Books care
            //este editata
            UpdateApartmentCategories(_context, selectedCategories, apartmentToUpdate);
            PopulateAssignedCategoryData(_context, apartmentToUpdate);
            return Page();
        }
    }
}
