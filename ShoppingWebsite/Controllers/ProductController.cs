using System.Diagnostics;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
namespace ShoppingWebsite.Controllers;

using Microsoft.EntityFrameworkCore;
using Models;
using TextContext;

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly Context _db;


    public ProductController(ILogger<ProductController> logger, Context dbContext)
    {
        _logger = logger;
        _db = dbContext;

    }
    public IActionResult Index()
    {

     

        return View();
    }



}

