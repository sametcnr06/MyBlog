using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Subscriber")]
public class SubscriberController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Abone Paneli";
        return View();
    }
}
