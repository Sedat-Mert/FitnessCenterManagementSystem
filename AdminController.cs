    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using sedatodev1.Models;

    namespace sedatodev1.Controllers
    {
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<Uye> _userManager;

        public AdminController(UserManager<Uye> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UyeIslemleri()
        {
            var uyeler = _userManager.Users.ToList();
            return View("UyeIslemleri", uyeler);
        }

        public async Task<IActionResult> UyeSil(string id)
        {
            var uye = await _userManager.FindByIdAsync(id);
            if (uye == null) return NotFound();

            await _userManager.DeleteAsync(uye);
            return RedirectToAction("UyeIslemleri");
        }

        //Uye ıle ılgılı duzenleme yapmak ıcın
        public async Task<IActionResult> UyeDuzenle(string id)
        {
            var uye = await _userManager.FindByIdAsync(id);
            if (uye == null) return NotFound();

            return View(uye);
        }
        [HttpPost]
        public async Task<IActionResult> UyeDuzenle(Uye model)
        {
            var uye = await _userManager.FindByIdAsync(model.Id);
            if (uye == null) return NotFound();

            // Güncelleme
            uye.UserName = model.UserName;
            uye.Email = model.Email;

            // Identity için gerekli normalization
            uye.NormalizedUserName = model.UserName.ToUpper();
            uye.NormalizedEmail = model.Email.ToUpper();

            var sonuc = await _userManager.UpdateAsync(uye);

            if (sonuc.Succeeded)
                return RedirectToAction("UyeIslemleri");

            foreach (var error in sonuc.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
    }
}
