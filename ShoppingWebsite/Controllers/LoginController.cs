using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Models;
using Request;
using TextContext;
namespace ShoppingWebsite.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly Context _db;


    public LoginController(ILogger<LoginController> logger, Context dbContext)
    {
        _logger = logger;
        _db = dbContext;

    }


    public IActionResult UserLogin()
    {

        // 获取名为"CookieName"的Cookie的值
        string cookieValue = Request.Cookies["UserVerify"];

        // 如果Cookie存在
        if (cookieValue != null)
        {
            return RedirectToAction("Index", "Home");
        }


        ViewBag.ProductClass = _db.ProductClass.ToList();
        return View();
    }

    public IActionResult Register()
    {

        // 获取名为"CookieName"的Cookie的值
        string cookieValue = Request.Cookies["UserVerify"];

        // 如果Cookie存在
        if (cookieValue != null)
        {
            return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Register", "Login");
        }
        password = this.ComputeMD5Hash(password);
        var user = new User
        {
            name = inputname,
            password = password,
        };
        _db.User.Add(user);
        _db.SaveChanges();



        return RedirectToAction("succ", "Login");

    }

    public IActionResult succ()
    {
        return View();
    }



        [HttpPost]
    public IActionResult UserLoginVerify()
    {



        string name = Request.Form["name"];
        string password = Request.Form["password"];
        var md5 = this.ComputeMD5Hash(name + password);

        password = this.ComputeMD5Hash(password);
        User verify = _db.User.FirstOrDefault(u => u.name == name);

        if (verify != null && verify?.password == password)
        {
            //設置cookies
            HttpContext.Response.Cookies.Append("UserVerify", verify.Id.ToString(), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
            //設置cookies
            HttpContext.Response.Cookies.Append("UserName", name, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
            return RedirectToAction("Index", "Home");
        }
        else
        {
            // 驗證失敗，重新導向到登入頁面並顯示錯誤訊息
            TempData["ErrorMessage"] = "登入失敗，請檢查您的帳號和密碼。";
            return RedirectToAction("UserLogin", "Login");
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
        Response.Cookies.Delete("UserVerify");
        Response.Cookies.Delete("UserName");

        return RedirectToAction("Index", "Home");
    }

}


