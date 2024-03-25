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

        ViewBag.Shop = _db.User.Where(u => u.Id == int.Parse(cookieValue)).ToList();

        return View();

    }


    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        //Task<IActionResult>
        var appSettings = Configuration.GetSection("ECPay");

        string ItemName = "shoppingprint";
        DateTime now = DateTime.Now;
        string formattedDateTime = now.ToString("yyyy/MM/dd HH:mm:ss");
        string MerchantTradeNo = "elim123123123";
        string TradeDesc = "shopping";
        
        int TotalAmount = 100;

        string input = "HashKey="+ appSettings["HashKey"] + "&ChoosePayment=ALL&EncryptType=1&ItemName="+ ItemName +"&MerchantID="+ appSettings["MerchantID"] + "&MerchantTradeDate="+ formattedDateTime + "&MerchantTradeNo="+ MerchantTradeNo + "&PaymentType=aio&ReturnURL="+ appSettings["ReturnURL"]+ "&TotalAmount="+ TotalAmount+ "&TradeDesc=" + TradeDesc + "&HashIV=" + appSettings["HashIV"];

        //Console.WriteLine(input);

        input = Encryption.CheckMacValue(input);
        // 创建要发送的 JSON 对象
        var jsonData = new
        {
            CheckMacValue = input,
            ChoosePayment = "ALL",
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
            return Content(htmlContent, "text/html");
        }
        else
        {
            // 处理失败的情况
            return RedirectToAction("Index", "Home");
        }




    }



}

