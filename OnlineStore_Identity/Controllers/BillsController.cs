using Marten.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineStore_Identity.Models;
//using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Rotativa.AspNetCore;
using System.Runtime.CompilerServices;

namespace OnlineStore_Identity.Controllers
{
    public class BillsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        HttpClient client = new HttpClient();

        public BillsController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public class RootObject
        {
            public string Metadata { get; set; }
            public int addressID { get; set; }
            public int billID { get; set; }
            public List<Cart> Value { get; set; }
        }

        public class BillDetailsRootObject
        {
            public string Metadata { get; set; }
            public int billID { get; set; }
            public double billSubTotal { get; set; }
            public double billTotal { get; set; }
            public DateTime billDate { get; set; }
            public string billNotes { get; set; } 
            public int addressID { get; set; }
            public int paymentID { get; set; }
            public string userID { get; set; }
            public Payment payment { get; set; }
            public Address address { get; set; }
            public List<BillProduct> billProducts { get; set; }
        }
        public IActionResult Index(int shippingID,int phone,string addressDetails,int paymentID,float tempTotal,float total)
        {
            //POST//Address => Payment => Bill => BillProduct
            string userID = _userManager.GetUserId(User);

            #region Address
            Address address = new Address() { shippingID = shippingID, addressDetails = addressDetails, addressPhone = phone };
            string _address = JsonConvert.SerializeObject(address);
            StringContent request = new StringContent(_address, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Addresses", request).Result;
            var myAddress = response.Content.ReadAsStringAsync().Result;
            RootObject addressRoot = JsonConvert.DeserializeObject<RootObject>(myAddress);
            int addressID = addressRoot.addressID;
            #endregion

            #region Bill
            Bill bill = new Bill() { paymentID = paymentID, billTotal = total, billSubTotal = tempTotal, billDate =  DateTime.Parse(DateTime.Now.ToShortDateString()), userID = userID, addressID = addressID };
            string _bill = JsonConvert.SerializeObject(bill);
            StringContent request2 = new StringContent(_bill, Encoding.UTF8, "application/json");
            HttpResponseMessage response2 = client.PostAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Bills", request2).Result;
            var myBill = response2.Content.ReadAsStringAsync().Result;
            RootObject billRoot = JsonConvert.DeserializeObject<RootObject>(myBill);
            int billID = billRoot.billID;
            #endregion

            #region BillProduct
            //StoreID To get all carts Products(For BillProduct)
            //foreach to post billProduct
            HttpResponseMessage response4 = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Carts?$expand=Store/Product&$filter=userID eq '{userID}'").Result;
            string carts = response4.Content.ReadAsStringAsync().Result;
            RootObject cartList = JsonConvert.DeserializeObject<RootObject>(carts);
            List<Cart> myCarts = cartList.Value;
            foreach (var item in myCarts)
            {
                BillProduct billProduct = new BillProduct()
                {
                    billID = billID,
                    storeID = item.storeID,
                    productID = item.Store.productID,
                    billProductQuantity = item.quantity,
                    billProductPrice = (item.Store.Product.productPrice) * (1 - item.Store.Product.productDiscount / 100)
                };
                string _billProduct = JsonConvert.SerializeObject(billProduct);
                StringContent request3 = new StringContent(_billProduct, Encoding.UTF8, "application/json");
                HttpResponseMessage response3 = client.PostAsync($"http://shirleyomda-001-site1.etempurl.com/odata/BillProducts", request3).Result;
                HttpResponseMessage response5 = client.DeleteAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Carts({item.cartID})").Result;

                #region Modifying store quantity after purchasing
                int removedQuantity = item.quantity ?? 0;
                int storeQuantity = item.Store.productQuantity ?? 0;
                storeQuantity -= removedQuantity;
                string _storeQuantityUpdate = JsonConvert.SerializeObject(new { productQuantity= storeQuantity });
                StringContent request6 = new StringContent(_storeQuantityUpdate, Encoding.UTF8, "application/json");
                HttpResponseMessage response6 = client.PatchAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Stores({item.storeID})", request6).Result; 
                #endregion

            }
            #endregion

            //return partial view and design modal
            //return RedirectToAction("CartList", "Carts");
            //return RedirectToAction("BillDetails", new { id = billID });
            //return Ok(new { id = billID });
            return Json(new { id = billID });
        }

        public IActionResult BillDetails(int id)
        {
            string u = _userManager.GetUserName(User);
            string[] userName = u.Split('@');
            ViewBag.User = userName[0];

            HttpResponseMessage response =client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Bills({id})?$expand=BillProducts/Product,Address/Shipping,Payment").Result;
            string bill = response.Content.ReadAsStringAsync().Result;
            BillDetailsRootObject myBill = JsonConvert.DeserializeObject<BillDetailsRootObject>(bill);
            return View(myBill);
        }

        public IActionResult printPdf(int id)
        {
            
            string u = _userManager.GetUserName(User);
            string[] userName = u.Split('@');
            ViewBag.User = userName[0];

            HttpResponseMessage response = client.GetAsync($"http://shirleyomda-001-site1.etempurl.com/odata/Bills({id})?$expand=BillProducts/Product,Address/Shipping,Payment").Result;
            string bill = response.Content.ReadAsStringAsync().Result;
            BillDetailsRootObject myBill = JsonConvert.DeserializeObject<BillDetailsRootObject>(bill);
            //return View(myBill);
            return new ViewAsPdf("BillDetails", myBill)
            {
                CustomSwitches = "--print-media-type --viewport-size 1024x768",
               
                //PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                //PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(7, 7, 7, 7),
                IsJavaScriptDisabled = false
            };

        }

    }
}
