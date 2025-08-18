using Eticaret.Core.Entities;
using Eticaret.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eticaret.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class AddressesController : Controller
    {
        private readonly DatabaseContext _db;
        public AddressesController(DatabaseContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var list = await _db.Addresses
                .Include(a => a.AppUser)
                .OrderByDescending(a => a.Id)
                .ToListAsync();
            return View(list);
        }

        // CREATE GET
        public IActionResult Create()
        {
            ViewBag.Users = _db.AppUsers
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = (x.Name + " " + x.Surname).Trim() + " - " + x.Email
                })
                .ToList();

            return View(new Address());
        }

        // CREATE POST
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Address model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Users = _db.AppUsers
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = (x.Name + " " + x.Surname).Trim() + " - " + x.Email
                    })
                    .ToList();
                return View(model);
            }

            bool userExists = await _db.AppUsers.AnyAsync(u => u.Id == model.AppUserId);
            if (!userExists)
            {
                ModelState.AddModelError(nameof(Address.AppUserId), "Geçerli bir kullanıcı seçin.");
                ViewBag.Users = _db.AppUsers
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = (x.Name + " " + x.Surname).Trim() + " - " + x.Email
                    })
                    .ToList();
                return View(model);
            }

            _db.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int id)
        {
            var address = await _db.Addresses.FindAsync(id);
            if (address is null) return NotFound();

            ViewBag.Users = _db.AppUsers
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = (x.Name + " " + x.Surname).Trim() + " - " + x.Email,
                    Selected = (x.Id == address.AppUserId)
                })
                .ToList();

            return View(address);
        }

        // EDIT POST
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Address model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Users = _db.AppUsers
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = (x.Name + " " + x.Surname).Trim() + " - " + x.Email,
                        Selected = (x.Id == model.AppUserId)
                    })
                    .ToList();
                return View(model);
            }

            bool userExists = await _db.AppUsers.AnyAsync(u => u.Id == model.AppUserId);
            if (!userExists)
            {
                ModelState.AddModelError(nameof(Address.AppUserId), "Geçerli bir kullanıcı seçin.");
                ViewBag.Users = _db.AppUsers
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = (x.Name + " " + x.Surname).Trim() + " - " + x.Email,
                        Selected = (x.Id == model.AppUserId)
                    })
                    .ToList();
                return View(model);
            }

            _db.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ✅ DETAILS GET
        public async Task<IActionResult> Details(int id)
        {
            var address = await _db.Addresses
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (address is null) return NotFound();

            return View(address);
        }

        // DELETE GET
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _db.Addresses
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (address is null) return NotFound();
            return View(address);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _db.Addresses.FindAsync(id);
            if (address is not null)
            {
                _db.Remove(address);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
