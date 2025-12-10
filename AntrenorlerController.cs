using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sedatodev1.Data;
using sedatodev1.Models;

namespace sedatodev1.Controllers
{
    public class AntrenorlerController : Controller
    {
        private readonly UygulamaDbContext _context;

        public AntrenorlerController(UygulamaDbContext context)
        {
            _context = context;
        }

        // LIST
        public IActionResult Index()
        {
            var list = _context.Antrenorler
                .Include(a => a.Salon)
                .ToList();

            return View(list);
        }

        // CREATE GET
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Salonlar = _context.Salonlar.ToList();
            return View();
        }

        // CREATE POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Antrenor model)
        {
            if (ModelState.IsValid)
            {
                _context.Antrenorler.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Salonlar = _context.Salonlar.ToList();
            return View(model);
        }

        // EDIT GET
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var ant = _context.Antrenorler.Find(id);
            if (ant == null) return NotFound();

            ViewBag.Salonlar = _context.Salonlar.ToList();
            return View(ant);
        }

        // EDIT POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Antrenor model)
        {
            if (ModelState.IsValid)
            {
                _context.Antrenorler.Update(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Salonlar = _context.Salonlar.ToList();
            return View(model);
        }

        // DELETE GET (ONAY SAYFASI)
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            // Cache kapat - geri tuşu Delete sayfasını tekrar göstermesin
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            var ant = _context.Antrenorler
                .Include(a => a.Salon)
                .FirstOrDefault(a => a.Id == id);

            if (ant == null) return NotFound();

            return View(ant);
        }

        // DELETE POST
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var ant = _context.Antrenorler.Find(id);
            if (ant == null) return NotFound();

            _context.Antrenorler.Remove(ant);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        // --------------------------------------------
        // HİZMET SEÇME (GET)
        // --------------------------------------------
        public IActionResult HizmetSec(int id)
        {
            var antrenor = _context.Antrenorler
                .Include(a => a.AntrenorHizmetler)
                .FirstOrDefault(a => a.Id == id);

            if (antrenor == null)
                return NotFound();

            ViewBag.Hizmetler = _context.Hizmetler
                .Include(h => h.Salon)
                .ToList();

            return View(antrenor);
        }

        // --------------------------------------------
        // HİZMET SEÇME (POST)
        // --------------------------------------------
        [HttpPost]
        public IActionResult HizmetSec(int antrenorId, List<int> hizmetler)
        {
            // Eski hizmetleri temizle
            var eskiKayitlar = _context.AntrenorHizmetler
                .Where(x => x.AntrenorId == antrenorId)
                .ToList();

            _context.AntrenorHizmetler.RemoveRange(eskiKayitlar);
            _context.SaveChanges();

            // Yeni hizmetleri ekle
            foreach (var hid in hizmetler)
            {
                _context.AntrenorHizmetler.Add(new AntrenorHizmet
                {
                    AntrenorId = antrenorId,
                    HizmetId = hid
                });
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
