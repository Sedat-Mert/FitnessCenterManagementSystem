using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sedatodev1.Data;
using sedatodev1.Models;

namespace sedatodev1.Controllers
{
    public class SalonlarController : Controller
    {
        private readonly UygulamaDbContext _context;

        public SalonlarController(UygulamaDbContext context)
        {
            _context = context;
        }

        // SALON LİSTESİ (HERKESE AÇIK)
        public IActionResult Index()
        {
            var salonlar = _context.Salonlar.ToList();
            return View(salonlar);
        }

        // ---------------- CREATE ----------------

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Salon model)
        {
            if (ModelState.IsValid)
            {
                _context.Salonlar.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // ---------------- EDIT ----------------

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var salon = _context.Salonlar.Find(id);
            if (salon == null) return NotFound();
            return View(salon);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Salon model)
        {
            if (ModelState.IsValid)
            {
                _context.Salonlar.Update(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // ---------------- DELETE (GET) ----------------

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            // Sayfanın cache’lenmesini engelle (geri tuşu sorun çıkarmasın)
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            var salon = _context.Salonlar.Find(id);
            if (salon == null) return NotFound();

            return View(salon); // Silmeden önce onay ekranı
        }

        // ---------------- DELETE CONFIRMED (POST) ----------------

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var salon = _context.Salonlar
                .Include(s => s.Antrenorler)
                .FirstOrDefault(s => s.Id == id);

            if (salon == null)
                return NotFound();

            // Bağlı antrenör varsa silme
            if (salon.Antrenorler.Any())
            {
                TempData["Hata"] = "Bu salon silinemez! Bu salona bağlı antrenörler var.";
                return RedirectToAction(nameof(Index));
            }

            _context.Salonlar.Remove(salon);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}

