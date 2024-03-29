using System.Diagnostics;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
namespace ShoppingWebsite.Controllers;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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

        var orderList = _db.ProductClass.ToList();

        ViewBag.Product = _db.Product.Where(u => u.manufacturer_id == int.Parse(cookieValue)).ToList();

        var newArray = new List<object>();
        foreach (var item in ViewBag.Product)
        {
            var product_class = orderList.FirstOrDefault(u => u.Id == item.product_class_id);

            // 從每個資料庫物件中提取資料並重新組成新的物件
            var newItem = new
            {
                name = item.name,
                price = item.price,
                product_class = product_class.class_name,
                Id = item.Id,
            };

            // 將新的物件加入新的陣列中
            newArray.Add(newItem);
        }



        ViewBag.Product = newArray;


        return View();
    }

    public IActionResult AddProduct()
    {

        string cookieValue = Request.Cookies["AdminVerify"];
        if (cookieValue == null)
        {
            TempData["ErrorMessage"] = "請先登入！";
            return RedirectToAction("Login", "Admin");
        }



        return View();
    }

    public IActionResult UpdProduct(int id)
    {

        string cookieValue = Request.Cookies["AdminVerify"];
        if (cookieValue == null)
        {
            TempData["ErrorMessage"] = "請先登入！";
            return RedirectToAction("Login", "Admin");
        }


        ViewBag.Product = _db.Product.Find(id);

        ViewBag.classname = _db.ProductClass.Find(ViewBag.Product.product_class_id);

        return View();
    }




    [HttpPost]
    public async Task<IActionResult> UpdateProduct(IFormCollection form,int id)
    {

        string cookieValue = Request.Cookies["AdminVerify"];
        string name = form["name"];
        string remarks = form["remarks"];
        int price = int.Parse(form["price"]);
        string product_class_name = form["product_class"];
        string image = "";

        int product_class_id = 1;
        var product_class = _db.ProductClass.FirstOrDefault(u => u.class_name == product_class_name);
        if (product_class == null)
        {
            var ProductClass = new ProductClass
            {
                class_name = product_class_name,
            };

            _db.ProductClass.Add(ProductClass);
            _db.SaveChanges();
            product_class_id = ProductClass.Id;
        }
        else
        {
            product_class_id = product_class.Id;
        }



        // 從表單中獲取圖片文件
        var imageFile = form.Files["imager"];

        if (imageFile != null && imageFile.Length > 0)
        {
            // 將圖片文件轉換為 base64 字符串
            using (var memoryStream = new MemoryStream())
            {
                image = "data:image/jpeg;base64,";
                await imageFile.CopyToAsync(memoryStream);
                var imageDataBytes = memoryStream.ToArray();
                image += Convert.ToBase64String(imageDataBytes);

                // 現在您可以將 base64 字符串保存到資料庫中，或者進行其他處理


            }
        }

        var product = _db.Product.Find(id);

        product.name = name;
        product.price = price;
        if(image != "")
        {
            product.imager = image;
        }
        

        product.remarks = remarks;
        product.product_class_id = product_class_id;

        _db.SaveChanges();






        return RedirectToAction("succ", "Admin");
    }






    [HttpPost]
    public async Task<IActionResult> PostProduct(IFormCollection form)
    {

        string cookieValue = Request.Cookies["AdminVerify"];
        string name = form["name"];
        string remarks = form["remarks"];
        int price = int.Parse(form["price"]);
        string product_class_name = form["product_class"];
        string image = "data:image/jpeg;base64,";

        int product_class_id = 1;
        var product_class = _db.ProductClass.FirstOrDefault(u => u.class_name == product_class_name);
        if (product_class == null)
        {
            var ProductClass = new ProductClass
            {
                class_name = product_class_name,
            };

            _db.ProductClass.Add(ProductClass);
            _db.SaveChanges();
            product_class_id = ProductClass.Id;
        }
        else
        {
            product_class_id = product_class.Id;
        }



        // 從表單中獲取圖片文件
        var imageFile = form.Files["imager"];

        if (imageFile != null && imageFile.Length > 0)
        {
            // 將圖片文件轉換為 base64 字符串
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                var imageDataBytes = memoryStream.ToArray();
                image += Convert.ToBase64String(imageDataBytes);

                // 現在您可以將 base64 字符串保存到資料庫中，或者進行其他處理


            }
        }


        var product = new Product
        {
            name = name,
            price = price,
            imager = image,
            remarks = remarks,
            product_class_id = product_class_id,
            manufacturer_id = int.Parse(Request.Cookies["AdminVerify"])

        };

        _db.Product.Add(product);
        _db.SaveChanges();






        return RedirectToAction("succ", "Admin");
    }


    

    public IActionResult ProductDelete(int id)
    {
        string cookieValue = Request.Cookies["AdminVerify"];
        if (cookieValue == null)
        {
            TempData["ErrorMessage"] = "請先登入！";
            return RedirectToAction("Login", "Admin");
        }


        var product = _db.Product.Find(id);
        int product_class_id = product.product_class_id;

        _db.Product.Remove(product);
        _db.SaveChanges();


        var product_class = _db.Product.FirstOrDefault(u => u.product_class_id == product_class_id);

        if(product_class == null)
        {
            var ProductClass = _db.ProductClass.Find(product_class_id);
            _db.ProductClass.Remove(ProductClass);
            _db.SaveChanges();

        }



        return RedirectToAction("Product", "Admin");

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


    public IActionResult succ()
    {


        return View();

    }

}

