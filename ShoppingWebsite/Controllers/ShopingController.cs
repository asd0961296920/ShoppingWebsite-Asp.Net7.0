﻿using System.Diagnostics;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
namespace ShoppingWebsite.Controllers;

using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Models;
using Server;
using TextContext;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class ShopingController : Controller
{
    private readonly ILogger<ShopingController> _logger;
    private readonly Context _db;
    private readonly HttpClient _httpClient;
    public IConfiguration Configuration { get; }


    public ShopingController(ILogger<ShopingController> logger, Context dbContext, IConfiguration configuration)
    {
        _logger = logger;
        _db = dbContext;
        Configuration = configuration;
        _httpClient = new HttpClient();

    }
    public IActionResult Index()
    {
        string cookieValue = Request.Cookies["UserVerify"];
        int userId = int.Parse(cookieValue);
        int all_number = 0;
        ViewBag.Shop = _db.Shop.Where(u => u.user_id == int.Parse(cookieValue)).ToList();

        var newArray = new List<object>();
        foreach (var item in ViewBag.Shop)
        {
            var product = _db.Product.Find(item.product_id);
            all_number += item?.number * product?.price;

            // 從每個資料庫物件中提取資料並重新組成新的物件
            var newItem = new { number = item?.number, price = product?.price , name = product?.name,shop_id = item?.Id };

            // 將新的物件加入新的陣列中
            newArray.Add(newItem);
        }

        ViewBag.Shop = newArray;
        ViewBag.all_number = all_number;


        return View();

    }


    //產品購買
    [HttpPost]
    public IActionResult Shoping()
    {

        // 获取名为"CookieName"的Cookie的值
        string cookieValue = Request.Cookies["UserVerify"];

        // 如果Cookie不存在
        if (cookieValue == null)
        {
            TempData["shopping"] = true;

            TempData["shopping_text"] = "請先登入";
            TempData["shopping_body"] = "登入後才可購買";
            return RedirectToAction("UserLogin", "Login");
        }

        




        int user_id = int.Parse(Request.Cookies["UserVerify"]);
        int manufacturer_id = int.Parse(Request.Form["manufacturer_id"]);
        int number = int.Parse(Request.Form["number"]);
        int product_id = int.Parse(Request.Form["product_id"]);



        var shop = new Shop
        {
            user_id = user_id,
            manufacturer_id = manufacturer_id,
            product_id = product_id,
            number = number
        };

        _db.Shop.Add(shop);
        _db.SaveChanges();




        TempData["shopping_bool"] = true;
        TempData["shopping_text"] = "購買成功";
        TempData["shopping_body"] = "購買商品可以在購物車看見";
        return RedirectToAction("succ", "Product");


    }



    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        // 獲取當前時間
        DateTime currentTime = DateTime.Now;

        // 將時間轉換為數字格式（例如 Unix 時間戳）
        long numericTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();

        //內部產生訂單
        var order = new Order
        {
            order_number ="0000"+ Request.Cookies["UserVerify"] + Request.Form["all_number"]+ numericTime,
            adress = Request.Form["adress"],
            phone = Request.Form["phone"],
            price = int.Parse(Request.Form["all_number"]),
            user_id = int.Parse(Request.Cookies["UserVerify"]),
            manufacturer_id = 0,
            payment = false
        };

        _db.Order.Add(order);
        _db.SaveChanges();


        //訂單下的詳細物件
        var shop = _db.Shop.Where(u => u.user_id == int.Parse(Request.Cookies["UserVerify"])).ToList();
        foreach (var item in shop)
        {
            var product = _db.Product.Find(item.product_id);
            int product_number = product.price * item.number;
            var items = new Item
            {
                order_id = order.order_number,
                product_id = item.product_id,
                product_name = product.name,
                price = product_number,
                user_id = int.Parse(Request.Cookies["UserVerify"]),
                manufacturer_id = product.manufacturer_id
            };

            _db.Item.Add(items);
            _db.SaveChanges();
        }


        //訂單記錄完成之後刪除購物車
        var Shoping = _db.Shop.Where(u => u.user_id == int.Parse(Request.Cookies["UserVerify"])).ToList();
        _db.Shop.RemoveRange(Shoping); // 或者使用 _db.RemoveRange(Shoping);
        _db.SaveChanges();






        //連接綠界金流
        var appSettings = Configuration.GetSection("ECPay");

        string ItemName = "shopping";
        DateTime now = DateTime.Now;
        string formattedDateTime = now.ToString("yyyy/MM/dd HH:mm:ss");
        string MerchantTradeNo = order.order_number;
        string TradeDesc = "shopping";
        
        int TotalAmount = order.price;

        string input = "HashKey="+ appSettings["HashKey"] + "&ChoosePayment=Credit&EncryptType=1&ItemName=" + ItemName +"&MerchantID="+ appSettings["MerchantID"] + "&MerchantTradeDate="+ formattedDateTime + "&MerchantTradeNo="+ MerchantTradeNo + "&PaymentType=aio&ReturnURL="+ appSettings["ReturnURL"]+ "&TotalAmount="+ TotalAmount+ "&TradeDesc=" + TradeDesc + "&HashIV=" + appSettings["HashIV"];

        //Console.WriteLine(input);

        input = Encryption.CheckMacValue(input);
        // 创建要发送的 JSON 对象
        var jsonData = new
        {
            CheckMacValue = input,
            ChoosePayment = "Credit",
            EncryptType = 1,
            MerchantID = appSettings["MerchantID"],
            MerchantTradeDate = formattedDateTime,
            MerchantTradeNo = MerchantTradeNo,
            PaymentType = "aio",
            TotalAmount = TotalAmount,
            TradeDesc = TradeDesc,
            ItemName = ItemName,
            ReturnURL = appSettings["ReturnURL"]

        };
        //Console.WriteLine(jsonData);

        // 将 JSON 对象转换为 application/x-www-form-urlencoded 格式的字符串
        var formData = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "CheckMacValue", input },
                { "ChoosePayment", jsonData.ChoosePayment },
                { "EncryptType", jsonData.EncryptType.ToString() },
                { "MerchantID", jsonData.MerchantID },
                { "MerchantTradeDate", jsonData.MerchantTradeDate },
                { "MerchantTradeNo", jsonData.MerchantTradeNo },
                { "PaymentType", jsonData.PaymentType },
                { "TotalAmount", jsonData.TotalAmount.ToString() },
                { "TradeDesc", jsonData.TradeDesc },
                { "ItemName", jsonData.ItemName },
                { "ReturnURL", jsonData.ReturnURL }
            });
        //Console.WriteLine(formData);

        // 发送 POST 请求
        var response = await _httpClient.PostAsync(appSettings["PostUrl"], formData);


        //return Content(await formData.ReadAsStringAsync(), "application/x-www-form-urlencoded");

        // 处理响应
        if (response.IsSuccessStatusCode)
        {
            // 处理成功的情况
            var htmlContent = await response.Content.ReadAsStringAsync();
            return View("Checkout", htmlContent);
            //return Content(htmlContent, "text/html");

        }
        else
        {
            // 处理失败的情况
            return RedirectToAction("Index", "Home");
        }




    }


    public IActionResult Delete(int id)
    {

        var shop = _db.Shop.Find(id);
        _db.Shop.Remove(shop);
        _db.SaveChanges();

        return RedirectToAction("Index", "Shoping");
    }



}

