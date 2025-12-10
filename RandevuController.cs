using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sedatodev1.Data;
using sedatodev1.Models;

namespace sedatodev1.Controllers
{
    [Authorize]
    public class RandevuController : Controller
    {
        private readonly UygulamaDbContext _context;
        private readonly UserManager<Uye> _userManager;

        public RandevuController(UygulamaDbContext context, UserManager<Uye> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // -----------------------------
        // 1) HİZMET SEÇ
        // -----------------------------
        public IActionResult HizmetSec()
        {
            var hizmetler = _context.Hizmetler.Include(h => h.Salon).ToList();
            return View(hizmetler);
        }

        // -----------------------------
        // 2) ANTRANÖR SEÇ
        // -----------------------------
        public IActionResult AntrenorSec(int hizmetId)
        {
            var hizmet = _context.Hizmetler.Find(hizmetId);
            ViewBag.Hizmet = hizmet;

            var antrenorler = _context.Antrenorler
                .Include(a => a.Salon)
                .Where(a => a.Uzmanlik.ToLower() == hizmet.GerekenUzmanlik.ToLower())
                .ToList();

            return View(antrenorler);
        }

        // -----------------------------
        // 2.5) TARİH SEÇ
        // -----------------------------
        public IActionResult TarihSec(int antrenorId, int hizmetId)
        {
            ViewBag.AntrenorId = antrenorId;
            ViewBag.HizmetId = hizmetId;
            return View();
        }

        // -----------------------------
        // 3) SAAT SEÇ
        // -----------------------------
        public IActionResult SaatSec(int antrenorId, int hizmetId, DateTime tarih)
        {
            ViewBag.Antrenor = _context.Antrenorler.Find(antrenorId);
            ViewBag.Hizmet = _context.Hizmetler.Find(hizmetId);
            ViewBag.Tarih = tarih;

            // Varsayılan saatler
            var saatler = new List<TimeSpan>
            {
                TimeSpan.Parse("13:00"),
                TimeSpan.Parse("14:00"),
                TimeSpan.Parse("15:00"),
                TimeSpan.Parse("16:00"),
                TimeSpan.Parse("17:00"),
                TimeSpan.Parse("18:00"),
                TimeSpan.Parse("19:00"),
                TimeSpan.Parse("20:00"),
                TimeSpan.Parse("21:00"),
                TimeSpan.Parse("22:00"),
                TimeSpan.Parse("23:00"),
                TimeSpan.Parse("00:00")
            };

            // DOLU saatler
            var doluSaatler = _context.Randevular
                .Where(r => r.AntrenorId == antrenorId && r.Tarih == tarih)
                .Select(r => r.Saat)
                .ToList();

            // MÜSAİT saatler
            var musaitSaatler = saatler
                .Where(s => !doluSaatler.Contains(s))
                .ToList();

            return View(musaitSaatler);
        }

        // -----------------------------
        // 4) RANDEVU KAYDET
        // -----------------------------
        [HttpPost]
        public IActionResult Kaydet(Randevu model)
        {
            // Çakışma kontrolü
            bool cakisma = _context.Randevular.Any(r =>
                r.AntrenorId == model.AntrenorId &&
                r.Tarih == model.Tarih &&
                r.Saat == model.Saat
            );

            if (cakisma)
            {
                TempData["HATA"] = "Bu saat dolu, başka bir saat seçiniz.";
                return RedirectToAction("SaatSec", new
                {
                    antrenorId = model.AntrenorId,
                    hizmetId = model.HizmetId,
                    tarih = model.Tarih
                });
            }

            model.UyeId = _userManager.GetUserId(User);

            _context.Randevular.Add(model);
            _context.SaveChanges();

            TempData["BASARI"] = "Randevunuz başarıyla oluşturuldu!";

            return RedirectToAction("HizmetSec");
        }

        // -----------------------------
        // 5) TÜM RANDEVULAR (Admin + Üye)
        // -----------------------------
        public IActionResult Liste()
        {
            var list = _context.Randevular
                .Include(r => r.Hizmet)
                .Include(r => r.Antrenor)
                .Include(r => r.Uye)
                .OrderBy(r => r.Tarih)
                .ThenBy(r => r.Saat)
                .ToList();

            return View(list);
        }

        // -----------------------------
        // 6) ADMIN — RANDEVU SİL
        // -----------------------------
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var r = _context.Randevular.Find(id);
            if (r != null)
            {
                _context.Randevular.Remove(r);
                _context.SaveChanges();
            }

            return RedirectToAction("Liste");
        }

        // -----------------------------
        // 7) ADMIN — RANDEVU DÜZENLE
        // -----------------------------
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var r = _context.Randevular
                .Include(x => x.Antrenor)
                .Include(x => x.Hizmet)
                .Include(x => x.Uye)
                .FirstOrDefault(x => x.Id == id);

            if (r == null)
                return NotFound();

            return View(r);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Randevu model)
        {
            var r = _context.Randevular.FirstOrDefault(x => x.Id == model.Id);

            if (r == null)
                return NotFound();

            r.Tarih = model.Tarih;
            r.Saat = model.Saat;

            _context.SaveChanges();

            return RedirectToAction("Liste");
        }
    }
}
