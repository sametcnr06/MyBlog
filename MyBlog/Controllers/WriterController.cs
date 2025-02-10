using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Writer")]
public class WriterController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Yazar Paneli";
        return View();
    }
}
