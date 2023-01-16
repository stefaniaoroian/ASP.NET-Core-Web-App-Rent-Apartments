 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Apartments.Data;
using Apartments.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Apartments.Pages.Rentals
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly Apartments.Data.ApartmentsContext _context;

        public IndexModel(Apartments.Data.ApartmentsContext context)
        {
            _context = context;
        }

        public IList<Rental> Rental { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Rental != null)
            {
                Rental = await _context.Rental
                .Include(r => r.Apartment)
                .Include(r => r.Client).ToListAsync();
            }
        }
    }
}
