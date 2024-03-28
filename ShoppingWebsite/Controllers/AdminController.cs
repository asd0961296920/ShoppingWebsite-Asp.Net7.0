using System.Diagnostics;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
namespace ShoppingWebsite.Controllers;

using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Models;
using TextContext;

public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly Context _db;


    public AdminController(ILogger<AdminController> logger, Context dbContext)
    {
        _logger = logger;
        _db = dbContext;

    }

    public IActionResult Login()
    {

        string cookieValue = Request.Cookies["AdminVerify"];
        if (cookieValue != null)
        {
            return RedirectToAction("Index", "Admin");
        }
        return View();
    }

    public IActionResult Register()
    {
        string cookieValue = Request.Cookies["AdminVerify"];
        if (cookieValue != null)
        {
            return RedirectToAction("Index", "Admin");
        }
        return View();
    }

    public IActionResult Index()
    {

        string cookieValue = Request.Cookies["AdminVerify"];
        if (cookieValue == null)
        {
            TempData["ErrorMessage"] = "請先登入！";
            return RedirectToAction("Login", "Admin");
        }
        var orderList = _db.Order.ToList();
        ViewBag.Order = _db.Item.Where(u => u.manufacturer_id == int.Parse(cookieValue)).ToList();

        var newArray = new List<object>();
        foreach (var item in ViewBag.Order)
        {
            var order = orderList.FirstOrDefault(u => u.order_number == item.order_id);

            // 從每個資料庫物件中提取資料並重新組成新的物件
            var newItem = new {
                CreatedAt = item.CreatedAt,
                order_number = item.order_id,
                product_name = item.product_name,
                price = item.price,
                adress = order.adress,
                phone = order.phone
            };

            // 將新的物件加入新的陣列中
            newArray.Add(newItem);
        }
        ViewBag.Item = newArray;


        return View();
    }

    public IActionResult Product()
    {

        string cookieValue = Request.Cookies["AdminVerify"];
        if (cookieValue == null)
        {
            TempData["ErrorMessage"] = "請先登入！";
            return RedirectToAction("Login", "Admin");
        }

        return View();
    }

    

    [HttpPost]
    public IActionResult RegisterPost()
    {

        string inputname = Request.Form["name"];
        string password = Request.Form["password"];

        if (inputname == null || password == null || inputname == "" || password == "")
        {
            TempData["ErrorMessage"] = "帳號和密碼不能為空。";
            return RedirectToAction("Register", "Admin");
        }
        password = this.ComputeMD5Hash(password);
        var user = new Manufacturer
        {
            name = inputname,
            password = password,
        };
        _db.Manufacturer.Add(user);
        _db.SaveChanges();



        return RedirectToAction("Login", "Admin");

    }


    [HttpPost]
    public IActionResult UserLoginVerify()
    {



        string name = Request.Form["name"];
        string password = Request.Form["password"];
        var md5 = this.ComputeMD5Hash(name + password);

        password = this.ComputeMD5Hash(password);
        Manufacturer verify = _db.Manufacturer.FirstOrDefault(u => u.name == name);

        if (verify != null && verify?.password == password)
        {
            //設置cookies
            HttpContext.Response.Cookies.Append("AdminVerify", verify.Id.ToString(), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
            //設置cookies
            HttpContext.Response.Cookies.Append("AdminName", name, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
            return RedirectToAction("Index", "Admin");
        }
        else
        {
            // 驗證失敗，重新導向到登入頁面並顯示錯誤訊息
            TempData["ErrorMessage"] = "登入失敗，請檢查您的帳號和密碼。";
            return RedirectToAction("Login", "Admin");
        }



    }

    //使用md5，32位元小寫英文加密
    public string ComputeMD5Hash(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            // 將輸入字串轉換為位元組陣列
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // 計算 MD5 雜湊值
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // 將位元組陣列轉換為十六進制字串表示形式
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2")); // "x2" 表示使用小寫字母表示每個位元組的十六進制值
            }
            return sb.ToString(); // 將 StringBuilder 轉換為字串
        }
    }

    public IActionResult OutLogin()
    {
        Response.Cookies.Delete("AdminVerify");
        Response.Cookies.Delete("AdminName");

        return RedirectToAction("Login", "Admin");
    }

}

