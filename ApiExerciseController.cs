using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sedatodev1.Models;

public class ApiExerciseController : Controller
{
    private readonly HttpClient _http;

    public ApiExerciseController()
    {
        _http = new HttpClient();
    }

    public async Task<IActionResult> Index(string bodyFilter, string equipmentFilter)
    {
        string url = "https://raw.githubusercontent.com/yuhonas/free-exercise-db/main/dist/exercises.json";

        var response = await _http.GetStringAsync(url);
        var list = JsonConvert.DeserializeObject<List<ExerciseApiModel>>(response);

        // Dropdown için distinct listeler
        ViewBag.BodyParts = list.Select(x => x.bodyPart).Distinct().OrderBy(x => x).ToList();
        ViewBag.Equipments = list.Select(x => x.equipment).Distinct().OrderBy(x => x).ToList();

        // FİLTRELEME  
        if (!string.IsNullOrEmpty(bodyFilter))
            list = list.Where(x => (x.bodyPart ?? "").Contains(bodyFilter, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!string.IsNullOrEmpty(equipmentFilter))
            list = list.Where(x => (x.equipment ?? "").Contains(equipmentFilter, StringComparison.OrdinalIgnoreCase)).ToList();



        return View(list.Take(50).ToList());
    }

    

}
