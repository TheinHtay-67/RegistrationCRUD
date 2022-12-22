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
    public class UserController : Controller
    {
        private readonly AppDbContext _dbContext;

        public UserController(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
        }

        // GET: UserController
        public async Task<ActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                var lstUser = await _dbContext.Users.Where(x => x.IsDeleted == false).ToListAsync();
                return View(lstUser);
            }
            return RedirectToAction("Login", "Login");
        }

        // GET: UserController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                UserViewModel viewModel = new UserViewModel();
                var user = await _dbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                viewModel = ConvertToViewModel(user);
                return View(viewModel);
            }
            return RedirectToAction("Login", "Login");
        }

        // GET: UserController/GetUserList for pagination
        [HttpGet("{pageNo:int}/{rowCount:int}")]
        public async Task<IActionResult> GetUserList(int pageNo = 1, int rowCount = 10)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            UserViewModel user = new UserViewModel();

            var lst = await _dbContext.Users.Where(x => x.IsDeleted == false).Skip(pageNo * rowCount - rowCount).Take(rowCount).ToListAsync();

            foreach(var item in lst)
            {
                user.Id = item.Id;
                user.Name = item.Name;
                user.Email = item.Email;
                user.Phone = item.Phone;
                user.UserName = item.UserName;
                user.Password = item.Password;
                user.IsDeleted = item.IsDeleted;
                user.CreatedDate = item.CreatedDate;
                user.UpdatedDate = item.UpdatedDate;
                users.Add(user);
            }

            return View(users);
        }

        // GET: UserController/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel viewModel)
        {
            try
            {
                UserModel model = new UserModel();
                //model.Password = EncodePasswordToBase64(model.Password);
                model = ConverToModel(viewModel);
                model.Password = EncryptString(key, model.Password);
                model.IsDeleted = false;
                model.CreatedDate = DateTime.Now;

                var checkAlreadyExist = _dbContext.Users.Any(c => c.Email == viewModel.Email && c.UserName == viewModel.UserName);
                if (!checkAlreadyExist)
                {
                    await _dbContext.Users.AddAsync(model);
                    var count = await _dbContext.SaveChangesAsync();
                }
                RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(viewModel);
            }
            return RedirectToAction("Index");
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (id == null)
                {
                    return NotFound();
                }
                UserViewModel viewModel = new UserViewModel();

                var model = await (_dbContext.Users).FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
                if (model == null)
                {
                    return NotFound();
                }
                viewModel = ConvertToViewModel(model);
                viewModel.Password = DecryptString(key, viewModel.Password);
                return View(viewModel);
            }
            return RedirectToAction("Login", "Login");
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UserViewModel viewModel)
        {
            try
            {
                UserModel model = new UserModel();

                model = ConverToModel(viewModel);
                model.Password = EncryptString(key, model.Password);
                model.UpdatedDate = DateTime.Now;

                var checkExist = _dbContext.Users.Any(c => c.Id == model.Id && c.UserName == model.UserName);

                if (checkExist)
                {
                    _dbContext.Users.Update(model);
                    var count = await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, UserViewModel viewModel)
        {
            try
            {
                var item = await _dbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                item.IsDeleted = true;
                _dbContext.Users.Update(item);
                var count = await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public UserViewModel ConvertToViewModel(UserModel model)
        {
            UserViewModel viewModel = new UserViewModel();
            viewModel.Id = model.Id;
            viewModel.Name = model.Name;
            viewModel.Email = model.Email;
            viewModel.Phone = model.Phone;
            viewModel.UserName = model.UserName;
            viewModel.Password = model.Password;
            viewModel.IsDeleted = model.IsDeleted;
            viewModel.CreatedDate = model.CreatedDate;
            viewModel.UpdatedDate = model.UpdatedDate;
            return viewModel;
        }

        public UserModel ConverToModel(UserViewModel viewModel)
        {
            UserModel model = new UserModel();
            model.Id = viewModel.Id;
            model.Name = viewModel.Name;
            model.Email = viewModel.Email;
            model.Phone = viewModel.Phone;
            model.UserName = viewModel.UserName;
            model.Password = viewModel.Password;
            model.IsDeleted = viewModel.IsDeleted;
            model.CreatedDate = viewModel.CreatedDate;
            model.UpdatedDate = viewModel.UpdatedDate;
            return model;
        }

        public async Task<JsonResult> UserExist(string email, string username)
        {
            ResponeModel responseModel = new ResponeModel();
            var checkAlreadyExist =await _dbContext.Users.AnyAsync(c => c.Email == email && c.UserName == username);

            if (checkAlreadyExist)
            {
                responseModel.respCode = "999";
                responseModel.respMsg = "User already exist!!";
            }
            else
            {
                responseModel.respCode = "000";
                responseModel.respMsg = "";
            }
            return Json(responseModel);
        }
    }
}
