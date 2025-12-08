using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sedatodev1.Models;
using sedatodev1.ViewModels;

namespace sedatodev1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Uye> _userManager;
        private readonly SignInManager<Uye> _signInManager;

        public AccountController(UserManager<Uye> userManager, SignInManager<Uye> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ---------------- LOGIN GET ----------------
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ---------------- LOGIN POST ----------------
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Kullanıcıyı email ile bul
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Email veya şifre hatalı.");
                return View(model);
            }

            // Şifre kontrolü
            var result = await _signInManager.PasswordSignInAsync(user, model.Sifre, false, false);

            if (result.Succeeded)
            {
                // ROL KONTROLÜ — ADMIN İSE PANEL'E GÖNDER
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }

                // Üye ise ana sayfaya
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Email veya şifre hatalı.");
            return View(model);
        }


        // ---------------- REGISTER GET ----------------
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // ---------------- REGISTER POST ----------------
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı adı daha önce alınmış mı kontrol et
                var userNameCheck = await _userManager.FindByNameAsync(model.KullaniciAdi);

                if (userNameCheck != null)
                {
                    ModelState.AddModelError("KullaniciAdi", "Bu kullanıcı adı zaten alınmış.");
                    return View(model);
                }

                var yeniUye = new Uye
                {
                    UserName = model.KullaniciAdi,
                    Email = model.Email,
                    AdSoyad = model.AdSoyad
                };

                var result = await _userManager.CreateAsync(yeniUye, model.Sifre);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(yeniUye, "Uye");
                    return RedirectToAction("Login");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            return View(model);
        }


        // ---------------- LOGOUT ----------------
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
