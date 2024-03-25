using System.Diagnostics;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
namespace ShoppingWebsite.Controllers;

using Microsoft.EntityFrameworkCore;
using Models;
using TextContext;

public class ShopingController : Controller
{
    private readonly ILogger<ShopingController> _logger;
    private readonly Context _db;


    public ShopingController(ILogger<ShopingController> logger, Context dbContext)
    {
        _logger = logger;
        _db = dbContext;

    }
    public IActionResult Index()
    {

     

        return View();
    }



}

