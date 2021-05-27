using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class WishlistsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public WishlistsController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public class RootObject<T>
        {
            public string Metadata { get; set; }
            public IEnumerable<T> Value { get; set; }
        }

        HttpClient client = new HttpClient();


        [Authorize]
        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(User);

            HttpResponseMessage response = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/WishLists?$expand=Product/Reviews,Product/Category,Product/Stores&$filter=userID eq '{userId}'").Result;
            string wishResult = response.Content.ReadAsStringAsync().Result;
            RootObject<WishList> wishlists = JsonConvert.DeserializeObject<RootObject<WishList>>(wishResult);
           
            //HttpResponseMessage response2 = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Categories").Result;
            //string catResult = response2.Content.ReadAsStringAsync().Result;
            //RootObject<Category> catlists = JsonConvert.DeserializeObject<RootObject<Category>>(catResult);
            //ViewBag.catIDs = catlists.Value;
           
            //HttpResponseMessage response3 = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Reviews").Result;
            //string rateResult = response3.Content.ReadAsStringAsync().Result;
            //RootObject<Review> ratelists = JsonConvert.DeserializeObject<RootObject<Review>>(rateResult);
            //ViewBag.review = ratelists.Value;

            return View(wishlists.Value);
        }

        public IActionResult RemoveFromWishlist(int id)
        {
            if (id != 0)
            {
                HttpResponseMessage response = client.DeleteAsync($"http://shirleyomda-001-site1.etempurl.com/odata/WishLists({id})").Result;
            }
            return RedirectToAction("Wishlist");
        }

        public IActionResult AddOrRemove(int id,string act)
        {
            string userID = _userManager.GetUserId(User);

            HttpResponseMessage response = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/WishLists?$filter=userID eq '{userID}' and productID eq {id}").Result;
            string wishResult = response.Content.ReadAsStringAsync().Result;
            RootObject<WishList> wishlists = JsonConvert.DeserializeObject<RootObject<WishList>>(wishResult);
            int wishlistID = wishlists.Value.Select(w => w.wishListID).FirstOrDefault();

            if (act == "Delete")
            {
                HttpResponseMessage response2 = client.DeleteAsync($"http://shirleyomda-001-site1.etempurl.com/odata/WishLists({wishlistID})").Result;
            }
            else
            {
                if (wishlists.Value.Count() == 0)
                {
                    WishList wishlist = new WishList() { userID = userID, productID = id };
                    string _wishlist = JsonConvert.SerializeObject(wishlist);
                    StringContent request = new StringContent(_wishlist, Encoding.UTF8, "application/json");
                    HttpResponseMessage response2 = client.PostAsync($"http://shirleyomda-001-site1.etempurl.com/odata/WishLists", request).Result;
                }
            }

            return RedirectToAction("Wishlist");
        }

        public IActionResult Wishlist()
        {
            string userId = _userManager.GetUserId(User);

            HttpResponseMessage response = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/WishLists?$expand=Product/Reviews,Product/Category,Product/Stores&$filter=userID eq '{userId}'").Result;
            string wishResult = response.Content.ReadAsStringAsync().Result;
            RootObject<WishList> wishlists = JsonConvert.DeserializeObject<RootObject<WishList>>(wishResult);

            //HttpResponseMessage response2 = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Categories").Result;
            //string catResult = response2.Content.ReadAsStringAsync().Result;
            //RootObject<Category> catlists = JsonConvert.DeserializeObject<RootObject<Category>>(catResult);
            //ViewBag.catIDs = catlists.Value;

            //HttpResponseMessage response3 = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Reviews").Result;
            //string rateResult = response3.Content.ReadAsStringAsync().Result;
            //RootObject<Review> ratelists = JsonConvert.DeserializeObject<RootObject<Review>>(rateResult);
            //ViewBag.review = ratelists.Value;

            return PartialView(wishlists.Value);
        }

     
    }
}
