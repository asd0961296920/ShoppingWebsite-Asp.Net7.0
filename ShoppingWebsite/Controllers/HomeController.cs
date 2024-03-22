using System.Diagnostics;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
namespace ShoppingWebsite.Controllers;

using Microsoft.EntityFrameworkCore;
using Models;
using TextContext;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Context _db;


    public HomeController(ILogger<HomeController> logger, Context dbContext)
    {
        _logger = logger;
        _db = dbContext;

    }
    public IActionResult Index()
    {
        ViewBag.ProductClass = _db.ProductClass.ToList();

        ViewBag.Product = _db.Product.ToList();

        return View();
    }

    public IActionResult ProductClass(int id)
    {
        ViewBag.ProductClass = _db.ProductClass.ToList();
        ViewBag.Product = _db.Product.Where(x => x.product_class_id == id).ToList();

        return View();
    }

    public IActionResult Privacy()
    {
        ViewBag.ProductClass = _db.ProductClass.ToList();
        return View();
    }


}

