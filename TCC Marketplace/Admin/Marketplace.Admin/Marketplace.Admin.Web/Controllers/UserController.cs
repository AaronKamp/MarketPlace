using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Marketplace.Admin.Identity;
using Marketplace.Admin.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Marketplace.Admin.Business;
using Marketplace.Admin.ViewModels;

namespace Marketplace.Admin.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserManager _userService;

        public UserController(IUserManager userService)
        {
            _userService = userService;
        }
    
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        private UserGridPaginationModel GetUserList(int? page)
        {
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["userPageSize"]);

            var pageNo = page ?? 1;

            var userGridPaginationDto = _userService.GetUsers(pageSize, pageNo);

            var rowNo = ((userGridPaginationDto.CurrentPage - 1) * userGridPaginationDto.PageSize) + 1;

            UserGridPaginationModel ugpm = new UserGridPaginationModel
            {
                PageSize = userGridPaginationDto.PageSize,
                CurrentPage = userGridPaginationDto.CurrentPage,
                NoOfPages = userGridPaginationDto.NoOfPages,
                TotalRecord = userGridPaginationDto.TotalRecord,
                Users = (from u in userGridPaginationDto.Users
                         select new UserGridViewModel
                         {
                             RowNo = rowNo++,
                             Id = u.UserId,
                             UserName = u.UserName,
                             Email = u.Email
                         }).ToList()
            };
            return ugpm;
        }

        [HttpGet]
        public JsonResult GetUser(int id)
        {

            var user = _userService.GetUser(id);

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            return Json(userViewModel, JsonRequestBehavior.AllowGet);
        }

        //Get:form
        [HttpGet]
        public ActionResult AddNewUser()
        {
            return PartialView("_AddNewUser");
        }

        [HttpPost]
        public JsonResult AddNewUser(UserViewModel newUser)
        {

            try
            {
                IdentityResult result;

                if (newUser.Id == 0)
                {
                   
                    var user = new IdentityUser {
                        UserName = newUser.UserName,
                        Email = newUser.Email,
                        CreatedUser = User.Identity.Name,
                        CreatedDate = DateTime.Now,
                        UpdatedUser = User.Identity.Name,
                        UpdatedDate = DateTime.Now
                    };
                    result = _userService.Create(user, newUser.Password);
                }

                else
                {
                    var user = new IdentityUser {
                        Id = newUser.Id,
                        UserName = newUser.UserName,
                        Email = newUser.Email,
                        UpdatedUser = User.Identity.Name,
                        UpdatedDate = DateTime.Now
                    };
                    
                    result = _userService.Update(user, newUser.Password);
                }

                if (result.Succeeded)
                {
                    _userService.SaveUser();

                    var jsonResult = new { Status = "Ok", Message = "User details " + (newUser.Id == 0 ? "added" : "updated") + " successfully!" };
                    return Json(jsonResult, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var message = result.Errors.ToList();
                    var jsonResult = new { Status = "Error", Message = message };
                    return Json(jsonResult, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                var msg = new List<string>();
                msg.Add(ex.Message);

                var jsonResult = new { Status = "Failed", Message = msg };

                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsers(int? page)
        {
            var userList = GetUserList(page);
            return Json(userList, JsonRequestBehavior.AllowGet);
        }

    }
}
