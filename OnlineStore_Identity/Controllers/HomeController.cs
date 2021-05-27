using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnlineStore_Identity.Models;
using OnlineStore_Identity.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore_Identity.Controllers
{
    public static class bla
    {
        public static List<string> checkedColors { get; set; }
        public static List<int> checkedClasses { get; set; }
        public static List<string> checkedSizes { get; set; }

        //public static string firstColor { get; set; } = "";
        //public static int firstClass { get; set; } = 0;

    }

    public class HomeController : Controller
    {
        string url = "http://shirleyomda-001-site1.etempurl.com/odata/Products?$expand=Reviews,Category,Stores";

        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }


        public class RootObject<T>
        {
            public string Metadata { get; set; }
            public IEnumerable<T> Value { get; set; }
        }



        HttpClient client = new HttpClient();

        public IActionResult Index()
        {
            HttpResponseMessage classRes = client.GetAsync("http://shirleyomda-001-site1.etempurl.com/odata/Classes").Result;
            string classResult = classRes.Content.ReadAsStringAsync().Result;
            RootObject<Class> classes = JsonConvert.DeserializeObject<RootObject<Class>>(classResult);
            ViewBag.classes = classes.Value;

            HttpResponseMessage categoryRes = client.GetAsync("http://shirleyomda-001-site1.etempurl.com/odata/Categories").Result;
            string categoryResult = categoryRes.Content.ReadAsStringAsync().Result;
            RootObject<Category> categories = JsonConvert.DeserializeObject<RootObject<Category>>(categoryResult);
            ViewBag.categories = categories.Value;

            HttpResponseMessage colorRes = client.GetAsync("http://shirleyomda-001-site1.etempurl.com/odata/Stores").Result;
            string colorResult = colorRes.Content.ReadAsStringAsync().Result;
            RootObject<Store> colors = JsonConvert.DeserializeObject<RootObject<Store>>(colorResult);
            ViewBag.colors = colors.Value.Select(s => s.productColor).GroupBy(s => s).Select(s => s.First()).ToList();

            HttpResponseMessage sizeRes = client.GetAsync("http://shirleyomda-001-site1.etempurl.com/odata/Stores").Result;
            string sizeResult = sizeRes.Content.ReadAsStringAsync().Result;
            RootObject<Store> sizes = JsonConvert.DeserializeObject<RootObject<Store>>(sizeResult);
            ViewBag.sizes = sizes.Value.Select(s => s.productSize).GroupBy(s => s).Select(s => s.First()).ToList();

            string userID = _userManager.GetUserId(User) != null ? _userManager.GetUserId(User) : "";
            //HttpContext.Session.SetString("userID", userID);
            //CookieOptions options = new CookieOptions();
            //options.Expires = DateTime.Now.AddDays(1);
            //Response.Cookies.Append("userID", userID, options);

            //string stored = Request.Cookies["userID"];
            //string stored = HttpContext.Session.GetString("userID");


            //HttpResponseMessage response = client.GetAsync("http://shirleyomda-001-site1.etempurl.com/odata/Products").Result;
            HttpResponseMessage response = client.GetAsync(url).Result;
            string Result = response.Content.ReadAsStringAsync().Result;
            RootObject<Product> products = JsonConvert.DeserializeObject<RootObject<Product>>(Result);
            //products.Value.Select(p=>p.)
            return View(products.Value);

            //Byte[] b = System.IO.File.ReadAllBytes(@"D:\ITI\Projects\Project4\New Project Online\OnlineStore_Identity\OnlineStore_Identity\wwwroot\css\assets\Images\img1.jpg");   // You can use your own method over here.         
            //return File(b, "image/jpeg");

            //return PhysicalFile(@"D:\ITI\Projects\Project4\New Project Online\OnlineStore_Identity\OnlineStore_Identity\wwwroot\css\assets\Images\img1.jpg", "image/jpeg");

        }

        //string baseURL = "http://shirleyomda-001-site1.etempurl.com/odata/Products?$select=*";
        //string basURL = "http://shirleyomda-001-site1.etempurl.com/odata/Categories({id})/Products";
        [HttpGet]
        public IActionResult CategoryFilter(int id)
        {
            url += $"&$filter=categoryID eq {id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            //HttpResponseMessage response = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Categories({id})/Products").Result;
            string Result = response.Content.ReadAsStringAsync().Result;
            RootObject<Product> products = JsonConvert.DeserializeObject<RootObject<Product>>(Result);
            return PartialView("CategoryFilter", products.Value);
            //return new JsonResult(products.Value);
        }

        //[HttpGet]
        //public IActionResult ColorFilter(string[] colors)
        //{
        //    baseURL += $"&$expand=Stores&$filter=Stores/any(s : (s/productColor eq '{colors[0]}'";
        //    for (int i = 1; i < colors.Length; i++)
        //    {
        //        baseURL += $" or s/productColor eq '{colors[i]}'";
        //    }
        //    baseURL += "))";
        //    HttpResponseMessage response = client.GetAsync(baseURL).Result;
        //    string Result = response.Content.ReadAsStringAsync().Result;
        //    RootObject products = JsonConvert.DeserializeObject<RootObject>(Result);
        //    return PartialView("CategoryFilter", products.Value);
        //}

        [HttpGet]
        public IActionResult Filter(string[] sizes, string[] colors, int[] classes, int id)
        {

            if (classes.Length != 0)
            {
                if (url.Contains("&$filter=classID eq"))
                {
                    //url = url.Remove(url.IndexOf("))"));
                    url += $" or classID eq {classes[0]}";
                }
                else
                {
                    url += $"&$filter=classID eq {classes[0]}";
                }
                for (int i = 1; i < classes.Length; i++)
                {
                    url += $" or classID eq {classes[i]}";
                }
                //url += "))";
            }

            if (id != 0)
            {
                if (url.Contains("&$filter=classID eq"))
                {
                    url += $" and categoryID eq {id}";
                }
                else
                {
                    url += $"&$filter=categoryID eq {id}";
                }
            }

            if (colors.Length != 0)
            {
                if (url.Contains("&$filter="))
                {
                    url += $" and Stores/any(s : (s/productColor eq '{colors[0]}'";
                }
                else
                {
                    url += $"&$filter=Stores/any(s : (s/productColor eq '{colors[0]}'";
                }
                for (int i = 1; i < colors.Length; i++)
                {
                    url += $" or s/productColor eq '{colors[i]}'";
                }
                url += "))";
            }

            if (sizes.Length != 0)
            {
                if (url.Contains("&$filter=classID") || url.Contains("&$filter=categoryID"))
                {
                    if (url.Contains("Stores/any(s : (s/"))
                    {
                        url = url.Remove(url.IndexOf("))"));
                        url += $" or s/productSize eq '{sizes[0]}'";
                    }
                    else
                    {
                        url += $" and Stores/any(s : (s/productSize eq '{sizes[0]}'";
                    }
                }
                else
                {
                    if (url.Contains("Stores/any(s : (s/"))
                    {
                        url = url.Remove(url.IndexOf("))"));
                        url += $" or s/productSize eq '{sizes[0]}'";
                    }
                    else
                    {
                        url += $"&$filter=Stores/any(s : (s/productSize eq '{sizes[0]}'";
                    }
                }
                for (int i = 1; i < sizes.Length; i++)
                {
                    url += $" or s/productSize eq '{sizes[i]}'";
                }
                url += "))";
            }

            //if (sizes.Length != 0 || colors.Length != 0)
            //{
            //    baseURL += "&$expand=Stores&$filter=Stores/any(s : (s/";
            //    if (sizes.Length != 0)
            //    {
            //        baseURL += $"productSize eq '{sizes[0]}'";
            //        for (int i = 1; i < sizes.Length; i++)
            //        {
            //            baseURL += $" or s/productSize eq '{sizes[i]}'";
            //        }
            //    }
            //    if (colors.Length != 0)
            //    {
            //        int i;
            //        if (sizes.Length == 0)
            //        {
            //            baseURL += $"productColor eq '{colors[0]}'";
            //            for (i = 1; i < colors.Length; i++)
            //            {
            //                baseURL += $" or s/productColor eq '{colors[i]}'";
            //            }
            //        }
            //        else
            //        {
            //            for (i = 0; i < colors.Length; i++)
            //            {
            //                baseURL += $" or s/productColor eq '{colors[i]}'";
            //            }
            //        }
            //    }
            //    baseURL += "))";
            //}

            //if (classes.Length != 0)
            //{
            //    baseURL += $"&$filter=(classID eq {classes[0]}";
            //    for (int i = 1; i < classes.Length; i++)
            //    {
            //        baseURL += $" or classID eq {classes[i]}";
            //    }
            //    baseURL += ")";
            //}

            HttpResponseMessage response = client.GetAsync(url).Result;
            string Result = response.Content.ReadAsStringAsync().Result;
            RootObject<Product> products = JsonConvert.DeserializeObject<RootObject<Product>>(Result);
            return PartialView("CategoryFilter", products.Value);
        }

        //[HttpGet]
        //public IActionResult ColorFilter(string color,bool flag,bool secondFlag)
        //{
        //    //baseURL += "&$expand=Stores&$filter=Stores/any(s : (s/";
        //    if (flag)
        //    {
        //        bla.url += "&$filter=Stores/any(s : (s/";
        //        if (!secondFlag)
        //        {
        //            bla.firstColor = color;
        //            bla.url += $"productColor eq '{color}'))";
        //            //baseURL += $"productColor eq '{color}'";
        //            //baseURL += "))";
        //        }
        //        else /*if (flag && secondFlag)*/
        //        {
        //            int x = bla.url.IndexOf("))");
        //            string newURL = bla.url.Remove(x);
        //            newURL += $" or s/productColor eq '{color}'))";
        //            bla.url = newURL;

        //            //int x = baseURL.IndexOf("))");
        //            //string newURL = baseURL.Remove(x);
        //            //baseURL = newURL;
        //            //baseURL += $" or productColor eq '{color}'";
        //            //baseURL += "))";
        //        }
        //    }
        //    else
        //    {
        //        if (!flag && !secondFlag)
        //        {
        //            int x = bla.url.IndexOf($"&$filter=Stores/any(s : (s/productColor eq '{color}'))");
        //           string newURL = bla.url.Remove(x);
        //            bla.url = newURL;
        //            //int x = baseURL.IndexOf($"&$expand=Stores&$filter=Stores/any(s : (s/productColor eq '{color}'))");

        //            //int x = baseURL.IndexOf("&$expand=Stores&$filter=Stores/any(s : (s/");
        //            //string newURL= baseURL.Remove(x);
        //            //baseURL = newURL;
        //        }
        //        else
        //        {
        //            string[] vv;
        //            if (bla.firstColor != color)
        //            {
        //                vv = bla.url.Split($" or s/productColor eq '{color}'");
        //            }
        //            else
        //            {
        //                vv = bla.url.Split($"s/productColor eq '{color}' or");
        //            }
        //            bla.url = vv[0] + vv[1];

        //            //int x = bla.url.IndexOf($" or s/productColor eq '{color}'");
        //            //string newURL = bla.url.Remove(x);
        //            //newURL += "))";
        //            //bla.url = newURL;
        //            //int x = baseURL.IndexOf($" or productColor eq '{color}'");
        //            //baseURL.Remove(x);
        //            //baseURL += "))";
        //        }
        //    }
        //    //HttpResponseMessage response = client.GetAsync(baseURL).Result;
        //    HttpResponseMessage response = client.GetAsync(bla.url).Result;
        //    string Result = response.Content.ReadAsStringAsync().Result;
        //    RootObject products = JsonConvert.DeserializeObject<RootObject>(Result);
        //    return PartialView("CategoryFilter", products.Value);
        //}

        //[HttpGet]
        //public IActionResult classFilter(int myClass, bool flag, bool secondFlag)
        //{
        //    //string[] newID = myClass.Split("class");
        //    //myClass = newID[1];
        //    if (flag)
        //    {
        //        bla.url += "&$filter=classID eq";
        //        if (!secondFlag)
        //        {
        //            bla.firstClass = myClass;
        //            bla.url += $" {myClass}";
        //        }
        //        else
        //        {
        //            bla.url += $" or classID eq {myClass}";
        //        }
        //    }
        //    else
        //    {
        //        if (!secondFlag)
        //        {
        //            int x = bla.url.IndexOf($"&$filter=classID eq {myClass}");
        //            string newURL = bla.url.Remove(x);
        //            bla.url = newURL;
        //        }
        //        else
        //        {
        //            string[] vv;
        //            if (bla.firstClass == myClass)
        //            {
        //                vv = bla.url.Split($"classID eq {myClass} or");
        //            }
        //            else
        //            {
        //                vv=bla.url.Split($" or classID eq {myClass}");
        //            }
        //            bla.url = vv[0] + vv[1];
        //        }
        //    }


        //    HttpResponseMessage response = client.GetAsync(bla.url).Result;
        //    string Result = response.Content.ReadAsStringAsync().Result;
        //    RootObject products = JsonConvert.DeserializeObject<RootObject>(Result);
        //    return PartialView("CategoryFilter", products.Value);
        //}



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ProductDetails(int id)
        {
            string userID = _userManager.GetUserId(HttpContext.User);
            ViewBag.userID = userID;

            HttpResponseMessage response = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Products({id})?$expand=Category,WishLists,Class,Stores/Carts,Reviews/AspNetUser").Result;
            string Result = response.Content.ReadAsStringAsync().Result;
            HomeProductDetailsVM productDetails = JsonConvert.DeserializeObject<HomeProductDetailsVM>(Result);
            int rate = 0;
            if (productDetails.Reviews.Count != 0)
            {
                foreach (var review in productDetails.Reviews)
                {
                    rate += review.rate ?? 0;
                }
            }

            ViewBag.rate = productDetails.Reviews.Count == 0 ? 0 : rate / productDetails.Reviews.Count;
            ViewBag.wishlistColor = productDetails.WishLists.Where(s => s.userID == userID).FirstOrDefault() == null ? "btn-light" : "btn-danger";
            return PartialView(productDetails);
        }
    }
}