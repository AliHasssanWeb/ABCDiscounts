using ABC.Customer.Domain.Configuration;
using ABC.Customer.Domain.DataConfig;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.Customer.WebClient.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class CartController : Controller
    {
        public IActionResult AddToCart(CartDetail CartDetail)   
        {
            ///  var CurrentCart = HttpContext.Session.GetString("CurrentCart");
            var Count = HttpContext.Session.GetString("Count");
            try            
            {
                var listcart = GlobalAccess.listcart as List<CartDetail>;
                //listcart = List<CartDetail>();
                if (listcart.Count() > 0)
                {
                    listcart.Add(CartDetail);
                }
                else
                {
                    GlobalAccess.listcart.Add(CartDetail);
                }
               

                return RedirectToAction("Index", "Home", new { @area = "Home" });

                //if (CurrentCart == null)
                //{
                //    List<CartDetail> list = new List<CartDetail>();
                //    list.Add(CartDetail);
                //    string serialized = JsonConvert.SerializeObject(list);
                //    CurrentCart = serialized;
                //    ViewBag.Cart = list.Count();
                //    int increment = 1;
                //    HttpContext.Session.SetString("Count", increment.ToString());
                //}
                //else
                //{
                //    List<CartDetail> list = new List<CartDetail>();
                //    string CartList = JsonConvert.SerializeObject(list);
                //    HttpContext.Session.SetString("CurrentCart", CartList);
                //    list.Add(CartDetail);
                //    string serialized = JsonConvert.SerializeObject(list);
                //    CurrentCart = serialized;
                //    ViewBag.Cart = list.Count();

                //    Count = Convert.ToInt32(HttpContext.Session.SetString("Count")) + 1;                       
                //}


                //var CurrentCart = HttpContext.Session.GetString("CurrentCart");

                //HttpContext.Session.SetString("CurrentCart", data.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //private void AddToCartChild(string resourceId, ResourceEnums resourceType)
        //{
        //    var CurrentCart = HttpContext.Session.GetString("CurrentCart");
        //    if (CurrentCart == null)
        //    {
        //        List<CartItem> carts = new List<CartItem>();
        //        CartItem cartItem = new CartItem();
        //        cartItem.ResourceId = resourceId;
        //        cartItem.ResourceType = resourceType;
        //        carts.Add(cartItem);
        //        CurrentCart = carts.ToString();
        //    }
        //    else
        //    {
        //        //var carts = (List<CartItem>)CurrentCart;
        //        var carts = JsonConvert.DeserializeObject<List<CartItem>>(CurrentCart);
        //        //List<CartItem> carts = new List<CartItem>();
        //        CartItem cartItem = new CartItem();
        //        cartItem.ResourceId = resourceId;
        //        cartItem.ResourceType = resourceType;
        //        carts.Add(cartItem);
        //        CurrentCart = carts.ToString();
        //    }            
        //}
        //public List<CartItem> Cart
        //{ 
        //    get
        //    {          
        //        if (CurrentCart == null)
        //        {
        //            List<CartItem> items = new List<CartItem>();
        //            string CurrentCart = JsonConvert.SerializeObject(items);
        //            HttpContext.Session.SetString("CurrentCart", CurrentCart);
        //            return items;
        //        }
        //    }
        //}

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult testmycart()
        {

            var listcart = GlobalAccess.listcart as List<CartDetail>;
            return View();
        }
    }
}
