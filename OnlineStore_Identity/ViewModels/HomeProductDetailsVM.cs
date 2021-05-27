using OnlineStore_Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore_Identity.ViewModels
{
    public class HomeProductDetailsVM
    {
        public string metadata { get; set; }

        public int productID { get; set; }
        public string productName { get; set; }
        public string productBrand { get; set; }
        public string productMaterial { get; set; }
        public double productPrice { get; set; }
        public double productDiscount { get; set; }
        public string productDescription { get; set; }
        public int classID { get; set; }
        public int categoryID { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Store> Stores { get; set; }
        public List<WishList> WishLists { get; set; }

        public Class Class { get; set; }
        public Category Category { get; set; }
    }
}
