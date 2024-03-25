using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using TextContext;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

using System.Diagnostics;
using System.Net.NetworkInformation;

namespace ShoppingWebsite.Controllers
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ILogger<HeaderViewComponent> _logger;
        private readonly Context _db;

        public HeaderViewComponent(ILogger<HeaderViewComponent> logger, Context dbContext)
        {
            _logger = logger;
            _db = dbContext;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.ProductClass = _db.ProductClass.ToList();
            // 获取名为"CookieName"的Cookie的值
            string cookieValue = Request.Cookies["UserVerify"];
            
            // 如果Cookie存在
            if (cookieValue != null)
            {
                // 执行您的操作
                ViewBag.login = true;
                ViewBag.loginName = Request.Cookies["UserName"];
            }
            else
            {
                ViewBag.login = false;
            }






            return View("/Views/Shared/Components/Header.cshtml");
        }
    }
}
