using Microsoft.AspNetCore.Mvc.RazorPages;
using Apartments.Data;

namespace Apartments.Models
{
    public class ApartmentCategoriesPageModel : PageModel
    {
        public List<AssignedCategoryData> AssignedCategoryDataList;
        public void PopulateAssignedCategoryData(ApartmentsContext context, Apartment apartment)
        {
            var allCategories = context.Category;
            var apartmentCategories = new HashSet<int>(
                apartment.ApartmentCategories.Select(c => c.CategoryID)); //
            AssignedCategoryDataList = new List<AssignedCategoryData>();
            foreach (var cat in allCategories)
            {
                AssignedCategoryDataList.Add(new AssignedCategoryData
                {
                    CategoryID = cat.ID,
                    Name = cat.CategoryName,
                    Assigned = apartmentCategories.Contains(cat.ID)
                });
            }
        }
        public void UpdateApartmentCategories(ApartmentsContext context,
        string[] selectedCategories, Apartment apartmentToUpdate)
        {
            if (selectedCategories == null)
            {
                apartmentToUpdate.ApartmentCategories = new List<ApartmentCategory>();
                return;
            }
            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            var apartmentCategories = new HashSet<int>
            (apartmentToUpdate.ApartmentCategories.Select(c => c.Category.ID));
            foreach (var cat in context.Category)
            {
                if (selectedCategoriesHS.Contains(cat.ID.ToString()))
                {
                    if (!apartmentCategories.Contains(cat.ID))
                    {
                        apartmentToUpdate.ApartmentCategories.Add(
                        new ApartmentCategory
                        {
                            ApartmentID = apartmentToUpdate.ID,
                            CategoryID = cat.ID
                        });
                    }
                }
                else
                {
                    if (apartmentCategories.Contains(cat.ID))
                    {
                        ApartmentCategory courseToRemove
                        = apartmentToUpdate
                        .ApartmentCategories
                        .SingleOrDefault(i => i.CategoryID == cat.ID);
                        context.Remove(courseToRemove);
                    }
                }
            }
        }



    }
}
