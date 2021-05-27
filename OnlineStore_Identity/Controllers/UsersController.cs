using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using OnlineStore_Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore_Identity.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        HttpClient client = new HttpClient();

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public class OrderRootObject
        {
            public string Metadata { get; set; }
            public IEnumerable<Bill> Value { get; set; }
           
        }

        public IActionResult Index()
        {
            string userID = _userManager.GetUserId(User);
            HttpResponseMessage response = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Bills?$expand=Payment&$filter=userID eq '{userID}'").Result;
            string order = response.Content.ReadAsStringAsync().Result;
            OrderRootObject myOrder = JsonConvert.DeserializeObject<OrderRootObject>(order);
           
            return View(myOrder.Value);
        }

        public IActionResult Orders()
        {
            string userID = _userManager.GetUserId(User);
            HttpResponseMessage response = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Bills?$expand=Payment&$filter=userID eq '{userID}'").Result;
            string order = response.Content.ReadAsStringAsync().Result;
            OrderRootObject myOrder = JsonConvert.DeserializeObject<OrderRootObject>(order);
            return PartialView(myOrder);
        }


        [HttpGet]
        public IActionResult Profile()
        {
            string id = _userManager.GetUserId(User);
            //            HttpResponseMessage response = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/AspNetUsers?$filter=NormalizeName eq'{id}'").Result;
            HttpResponseMessage response = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/AspNetUsers('{id}')").Result;
            string Result = response.Content.ReadAsStringAsync().Result;
            AspNetUser useerRoot = JsonConvert.DeserializeObject<AspNetUser>(Result);
            return View(useerRoot);
        }

        [HttpPost]
        public ActionResult Profile(AspNetUser edituser)
        {
            //HttpResponseMessage response = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/AspNetUsers?$filter=NormalizedEmail eq'{edituser.Email.ToUpper()}'").Result;
            //string Result = response.Content.ReadAsStringAsync().Result;
            //RootObject<AspNetUser> useerRoot = JsonConvert.DeserializeObject<RootObject<AspNetUser>>(Result);
            //AspNetUser orginUser = useerRoot.Value.First();

            string userid = _userManager.GetUserId(User);

            //AspNetUser aspNetUser =new AspNetUser
            //{
            //    AccessFailedCount=orginUser.AccessFailedCount,
            //    ConcurrencyStamp = orginUser.ConcurrencyStamp,
            //    EmailConfirmed = orginUser.EmailConfirmed,
            //    LockoutEnabled = orginUser.LockoutEnabled,
            //    LockoutEnd = orginUser.LockoutEnd,
            //    Id = orginUser.Id,
            //    Email = edituser.Email,
            //    NormalizedEmail = edituser.Email.ToUpper(),
            //    NormalizedUserName = edituser.UserName.ToUpper(),
            //    UserName = edituser.UserName,
            //    PasswordHash = orginUser.PasswordHash,
            //    PhoneNumber= edituser.PhoneNumber,
            //    PhoneNumberConfirmed =orginUser.PhoneNumberConfirmed,
            //    SecurityStamp=orginUser.SecurityStamp,
            //    TwoFactorEnabled=orginUser.TwoFactorEnabled
            //};
            string data = JsonConvert.SerializeObject(
            new
            {
            Email = edituser.Email,
            NormalizedEmail = edituser.Email.ToUpper(),
            NormalizedUserName = edituser.UserName.ToUpper(),
            UserName = edituser.UserName,
            PhoneNumber = edituser.PhoneNumber
            }
            );
            StringContent reqBody = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = client.PatchAsync($"http://shirleyomda-001-site1.etempurl.com/odata/AspNetUsers('{userid}')", reqBody).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.Message = "added";
            }
            else
            {
                ViewBag.Message = "Error not added";
            }
            //            return RedirectToAction("Index");
            return RedirectToAction("Index", new RouteValueDictionary(
               new { controller = "Home", action = "Index"}));

        }
    }
}
