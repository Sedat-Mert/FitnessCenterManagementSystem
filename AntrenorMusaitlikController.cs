using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sedatodev1.Data;
using sedatodev1.Models;

namespace sedatodev1.Controllers
{
    public class AntrenorMusaitlikController : Controller
    {
        private readonly UygulamaDbContext _context;

        public AntrenorMusaitlikController(UygulamaDbContext context)
        {
            _context = context;
        }

        // LIST
        public IActionResult Index(int antrenorId)
        {
            var ant = _context.Antrenorler.Find(antrenorId);
            if (ant == null) return NotFound();

            ViewBag.Antrenor = ant;

            var liste = _context.AntrenorMusaitlikler
                .Where(x => x.AntrenorId == antrenorId)
                .ToList();

            return View(liste);
        }

        // CREATE GET
        public IActionResult Create(int antrenorId)
        {
            ViewBag.AntrenorId = antrenorId;
            return View();
        }

        // CREATE POST
        [HttpPost]
        public IActionResult Create(AntrenorMusaitlik model)
        {
            if (ModelState.IsValid)
            {
                _context.AntrenorMusaitlikler.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index", new { antrenorId = model.AntrenorId });
            }

            return View(model);
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            var item = _context.AntrenorMusaitlikler.Find(id);
            if (item == null) return NotFound();

            int antrenorId = item.AntrenorId;

            _context.AntrenorMusaitlikler.Remove(item);
            _context.SaveChanges();

            return RedirectToAction("Index", new { antrenorId });
        }
    }
}
