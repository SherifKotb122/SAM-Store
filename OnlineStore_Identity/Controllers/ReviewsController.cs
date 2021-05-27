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
    public class ReviewsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewsController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public class RootObject<T>
        {
            public string Metadata { get; set; }
            public IEnumerable<T> Value { get; set; }
        }

        HttpClient client = new HttpClient();

        public List<Product> GetProductsToReview()
        {
            string userID = _userManager.GetUserId(User);
            // get list of billproducts for this user
            // get list of reviews for this product
            // create list of billproducts that are not in the reviews
            HttpResponseMessage response = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Bills?$expand=BillProducts/Product,BillProducts/Store&$filter=userID eq '{userID}'&$select=BillProducts/Store,BillProducts/Product").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            RootObject<Bill> bills = JsonConvert.DeserializeObject<RootObject<Bill>>(result);

            List<Product> toReviewProducts = bills.Value.SelectMany(b => b.BillProducts).Select(b => b.Product).ToList();

            HttpResponseMessage response2 = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Reviews?$expand=Product&$filter=userID eq '{userID}'").Result;
            string result2 = response2.Content.ReadAsStringAsync().Result;
            RootObject<Review> reviews = JsonConvert.DeserializeObject<RootObject<Review>>(result2);

            List<Product> inReviewProducts = reviews.Value.Select(b => b.Product).ToList();

            return toReviewProducts.Where(p => !inReviewProducts.Any(p2 => p2.productID == p.productID)).GroupBy(p => p.productID).Select(p => p.First()).ToList();
        }

        public IActionResult Index()
        {
            return View(GetProductsToReview());
        }

        public IActionResult ProductsToReview()
        {
            return PartialView(GetProductsToReview());
        }

        public IActionResult AddReview(int id, string review, int rate)
        {
            string userID = _userManager.GetUserId(User);
            string rev = JsonConvert.SerializeObject(new Review() { userID = userID, productID = id, rate = rate, reviewNotes = review });
            StringContent request = new StringContent(rev, Encoding.UTF8, "application/json");
            HttpResponseMessage content = client.PostAsync("http://shirleyomda-001-site1.etempurl.com/odata/Reviews", request).Result;
            if (content.IsSuccessStatusCode)
            {
                return RedirectToAction("ProductsToReview");
            }

            return BadRequest();
        }
    }
}
