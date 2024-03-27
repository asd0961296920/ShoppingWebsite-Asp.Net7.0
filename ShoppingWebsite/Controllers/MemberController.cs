using System.Diagnostics;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
namespace ShoppingWebsite.Controllers;

using Microsoft.EntityFrameworkCore;
using Models;
using TextContext;

public class MemberController : Controller
{
    private readonly ILogger<MemberController> _logger;
    private readonly Context _db;


    public MemberController(ILogger<MemberController> logger, Context dbContext)
    {
        _logger = logger;
        _db = dbContext;

    }
    public IActionResult Index()
    {
        string cookieValue = Request.Cookies["UserVerify"];
        if (cookieValue == null)
        {
            return RedirectToAction("Index", "Home");
        }
        ViewBag.Order = _db.Order.Where(u => u.user_id == int.Parse(cookieValue)).ToList();

        return View();
    }




}

