using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Editor")]
public class EditorController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Editör Paneli";
        // Sadece Index sayfasına yönlendiriyor
        return View();
    }
}
