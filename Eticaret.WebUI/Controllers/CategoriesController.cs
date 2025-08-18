using Eticaret.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eticaret.WebUI.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DatabaseContext _context;

        public CategoriesController(DatabaseContext context)
        {
            _context = context;
        }

        // /Categories/Index/{id}
        public async Task<IActionResult> IndexAsync(int? id)
        {
            if (id == null)
                return NotFound();

            // 1) Ana kategoriyi başlık için al
            var category = await _context.Categories
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(c => c.Id == id.Value);
            if (category == null)
                return NotFound();

            // 2) Alt kategori id'lerini al (tek seviye)
            var childIds = await _context.Categories
                                         .Where(c => c.ParentId == id.Value)
                                         .Select(c => c.Id)
                                         .ToListAsync();

            // 3) Kendisi + altlar = kapsama kümesi
            var allIds = new List<int> { id.Value };
            allIds.AddRange(childIds);

            // 4) Ürünleri (CategoryId nullable olabilir) getir
            var products = await _context.Products
                                         .Where(p => p.CategoryId.HasValue &&
                                                     allIds.Contains(p.CategoryId.Value))
                                         .OrderByDescending(p => p.Id)
                                         .ToListAsync();

            // 5) View için kategori bilgisini ver
            ViewBag.Category = category;

            // View: @model List<Eticaret.Core.Entities.Product>
            return View(products);
        }
    }
}
