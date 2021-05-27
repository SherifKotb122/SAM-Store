using OnlineStore_Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineStore_Identity.ViewModels
{
    public class ProductBillVM
    {
        //public List<Product> products = new List<Product>();
        public IEnumerable<Product> products;
        public IEnumerable<Bill> bills;
        public IEnumerable<BillProduct> billProducts;
        public IEnumerable<Class> classes;
        public IEnumerable<Category> categories;
        public IEnumerable<Store> stores;
        public decimal stock =0;
        public decimal billQuan =0;
        public decimal storeQuan = 0;
        public decimal makam = 1;
        public int total;
        public string className;
        public string categoryName;

        //public Bill bills = new Bill();
        public double todayProfit=0;
        public double allProfit=0;
        public double[] lastMonthProfit=new double[31];
        public double[] lastYearProfit = new double[12];
    }
}
