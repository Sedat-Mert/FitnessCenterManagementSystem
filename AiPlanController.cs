using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sedatodev1.Models;
using sedatodev1.Services;

namespace sedatodev1.Controllers
{
    [Authorize(Roles = "Uye")]
    public class AiPlanController : Controller
    {
        private readonly GeminiAiService _geminiAiService;

        public AiPlanController(GeminiAiService geminiAiService)
        {
            _geminiAiService = geminiAiService;
        }

        // AI Plan formunu gösterir
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // FORM POST 
        [HttpPost]
        public async Task<IActionResult> Index(AiPlanRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Gemini AI çağrısı
            var sonuc = await _geminiAiService.HaftalikPlanOlusturAsync(
                model.Boy,
                model.Kilo,
                model.VucutTipi,
                model.Hedef
            );

            // Sonucu ViewBag ile View'a gönderiyoruz
            ViewBag.AiSonuc = sonuc;

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Models()
        {
            var txt = await _geminiAiService.ModelListeleAsync();
            return Content(txt, "text/plain; charset=utf-8");
        }


    }
}
