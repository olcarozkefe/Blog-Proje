using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AdminBlog.Models;

namespace AdminBlog.Controllers;

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
        if (!HttpContext.Session.GetInt32("Id").HasValue)
        {
            return RedirectToAction(nameof(Login));
        }

        return View();
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

   public IActionResult Category()
    {
        if (!HttpContext.Session.GetInt32("Id").HasValue)
        {
            return RedirectToAction(nameof(Login));
        }
        List<Category> list = _context.Category.ToList();
        return View(list);
    } 

    public async Task<IActionResult> AddCategory(Category category)
    {
       if (!HttpContext.Session.GetInt32("Id").HasValue)
        {
            return RedirectToAction(nameof(Login));
        }

       if (category.Id == 0)
       {
            await _context.AddAsync(category);
       }
       else
       {
        _context.Update(category);
       }



        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Category));
    }


    public async Task<IActionResult> DeleteCategory(int? Id)
    {
        if (!HttpContext.Session.GetInt32("Id").HasValue)
        {
            return RedirectToAction(nameof(Login));
        }
        
        Category category = await _context.Category.FindAsync(Id);
        _context.Remove(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(category));
    }

    public async Task<IActionResult> CategoryDetails(int Id)
    {
        if (!HttpContext.Session.GetInt32("Id").HasValue)
        {
            return RedirectToAction(nameof(Login));
        }

        var category = await _context.Category.FindAsync(Id);
        return Json(category);
    } 


    public IActionResult Author()
    {
        if (!HttpContext.Session.GetInt32("Id").HasValue)
        {
            return RedirectToAction(nameof(Login));
        }

        List<Author> list = _context.Author.ToList();
        return View(list);
    } 


      public async Task<IActionResult> AddAuthor(Author author)
    {
        if (!HttpContext.Session.GetInt32("Id").HasValue)
        {
            return RedirectToAction(nameof(Login));
        }


        if (author.Id == 0)
        {
            await _context.AddAsync(author);
        }
        else
        {
            _context.Update(author);
        }
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Author));
    } 



     public async Task<IActionResult> AuthorDetails(int Id)
    {
        if (!HttpContext.Session.GetInt32("Id").HasValue)
        {
            return RedirectToAction(nameof(Login));
        }

        var author = await _context.Author.FindAsync(Id);
        return Json(author);
    } 


      public async Task<IActionResult> DeleteAuthor(int? Id)
    {
        if (!HttpContext.Session.GetInt32("Id").HasValue)
        {
            return RedirectToAction(nameof(Login));
        }

        var author = await _context.Author.FindAsync(Id);
        _context.Remove(author);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Author));
    }



    public IActionResult Login()
    {
        return View();
    }


    public IActionResult LoginKontrol(string Email, string Password)
    {
        var author = _context.Author.FirstOrDefault(w => w.Email == Email && w.Password == Password);

        if (author == null)
        {   
            return RedirectToAction(nameof(Login));
        }

        HttpContext.Session.SetInt32("Id", author.Id);
        return RedirectToAction(nameof(Category));
    }


     public IActionResult Logout()
    {
       HttpContext.Session.Remove("Id");
        return RedirectToAction(nameof(Login));
    }
}
