using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SiteBlog.Models;

namespace SiteBlog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BlogContext _context;

    public HomeController(ILogger<HomeController> logger, BlogContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
         var list = _context.Blog.Take(24).Where(b => b.IsPublish).OrderByDescending(x => x.CreateTime).ToList();
          foreach (var blog in list)
          {
             blog.Author = _context.Author.Find(blog.AuthorId);
          }
          return View(list); 
    }

    public IActionResult Post(int Id)
    {
        var blog = _context.Blog.Find(Id);
        blog.Author = _context.Author.Find(blog.AuthorId);
        blog.ImagePath = "/image/" + blog.ImagePath;
        return View(blog);
    } 

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
