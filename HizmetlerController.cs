using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sedatodev1.Data;
using sedatodev1.Models;

namespace sedatodev1.Controllers
{
    public class HizmetlerController : Controller
    {
        private readonly UygulamaDbContext _context;

        public HizmetlerController(UygulamaDbContext context)
        {
            _context = context;
        }

        // ---------------- LIST ----------------
        public IActionResult Index()
        {
            var hizmetler = _context.Hizmetler
                .Include(h => h.Salon)
                .ToList();

            return View(hizmetler);
        }

        // ---------------- CREATE ----------------

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Salonlar = _context.Salonlar.ToList();
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Hizmet model)
        {
            ModelState.Remove("Salon");
            ModelState.Remove("AntrenorHizmetler");
            ModelState.Remove("Randevular");

            if (ModelState.IsValid)
            {
                // 🔥 GerekenUzmanlik boş bırakıldıysa otomatik doldur
                if (string.IsNullOrWhiteSpace(model.GerekenUzmanlik))
                {
                    model.GerekenUzmanlik = model.Ad.ToLower();
                }

                _context.Hizmetler.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Salonlar = _context.Salonlar.ToList();
            return View(model);
        }


        // ---------------- EDIT ----------------

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet == null) return NotFound();

            ViewBag.Salonlar = _context.Salonlar.ToList();
            return View(hizmet);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Hizmet model)
        {
            ModelState.Remove("Salon");
            ModelState.Remove("AntrenorHizmetler");
            ModelState.Remove("Randevular");

            if (ModelState.IsValid)
            {
                _context.Hizmetler.Update(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Salonlar = _context.Salonlar.ToList();
            return View(model);
        }


        // ---------------- DELETE (GET) ----------------

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            // Cache'i kapat – geri tuşu hata vermesin
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            var hizmet = _context.Hizmetler
                .Include(h => h.Salon)
                .FirstOrDefault(h => h.Id == id);

            if (hizmet == null) return NotFound();

            return View(hizmet);
        }

        // ---------------- DELETE CONFIRMED (POST) ----------------

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet == null) return NotFound();

            _context.Hizmetler.Remove(hizmet);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
