using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RegistrationTest20221221.Context;
using RegistrationTest20221221.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RegistrationTest20221221.Common.Common;

namespace RegistrationTest20221221.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _dbContext;

        public LoginController(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserViewModel viewModel)
        {
            var password = EncryptString(key, viewModel.Password);
            var checkLogin = await _dbContext.Users.AnyAsync(c => c.UserName == viewModel.UserName && c.Password == password && c.IsDeleted == false);
            if (checkLogin)
            {
                HttpContext.Session.SetString("UserName", viewModel.UserName);
                return RedirectToAction("Index", "User");
            }
            else
            {
                ViewBag.Message = "User name and password incorrect!!!";
                return View();
            }
        }
        #region
        //// GET: LoginController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: LoginController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: LoginController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: LoginController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: LoginController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: LoginController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: LoginController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: LoginController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        #endregion
    }
}
