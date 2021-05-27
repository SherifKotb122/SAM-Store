using Microsoft.AspNetCore.Http;
using OnlineStore_Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
namespace OnlineStore_Identity.ViewModels
{
    public class StoreItemVM
    {
        public IFormFile file { get; set; }
        public Nullable<int> productID { get; set; }
        public string productColor { get; set; }
        public string productSize { get; set; }
        //public byte[] productImage { get; set; }

        public Nullable<int> productQuantity { get; set; }
        public int ID { get; set; }
   }
}