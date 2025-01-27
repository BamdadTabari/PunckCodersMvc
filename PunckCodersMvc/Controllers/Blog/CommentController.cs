using Microsoft.AspNetCore.Mvc;

namespace PunckCodersMvc.Controllers.Blog;
public class CommentController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
