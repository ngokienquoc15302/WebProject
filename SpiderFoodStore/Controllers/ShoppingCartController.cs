using SpiderFoodStore.Data;
using SpiderFoodStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VegetableStore.Others;

namespace SpiderFoodStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        SpiderFoodStoreContext db = new SpiderFoodStoreContext();
        private List<ShoppingCart> GetCart()
        {
            return Session["Cart"] as List<ShoppingCart>;
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Product");
            }
            List<ShoppingCart> cart = GetCart();
            return View(cart);
        }
        public ActionResult AddToCart(int? id, int? newAmount)
        {
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new List<ShoppingCart>();
            }
            List<ShoppingCart> cart = GetCart();
            if(db.Products.Where(n => n.Id == id).FirstOrDefault() == null)
            {
                return HttpNotFound();
            }
            ShoppingCart item = cart.FirstOrDefault(n => n.Id == id);
            if(item == null)
            {
                item = new ShoppingCart((int)id);
                if (newAmount != null && newAmount > 0)
                {
                    item.Amount = (int)newAmount;
                }
                cart.Add(item);
            }
            else
            {
                item.Amount += (newAmount != null && newAmount > 0) ? (int)newAmount : 1;
            }
            return RedirectToAction("Index");
        }
        public ActionResult RemoveFromCart(int? id)
        {
            if(db.Products.FirstOrDefault(n => n.Id == id) == null)
            {
                return HttpNotFound();
            }
            List<ShoppingCart> cart = GetCart();
            ShoppingCart item = cart.FirstOrDefault(n => n.Id == id);
            if(null != item)
            {
                cart.RemoveAll(n => n.Id == id);
            }
            if(cart.Count > 0)
            {
                return RedirectToAction("Index");
            }
            Session["Cart"] = null;
            return RedirectToAction("Index", "Product");
        }
        public ActionResult UpdateCart(FormCollection frm)
        {
            List<ShoppingCart> cart = GetCart();
            foreach(ShoppingCart item in cart)
            {
                //Giống dùng map :)))) VD: địa chỉ là "cart2" và giá trị là 4 thì sản phẩm có id = 2 sẽ cập nhật số lượng thành 4
                item.Amount = int.Parse(frm["cart" + item.Id].ToString());
            }
            cart.RemoveAll(n => n.Amount <= 0);
            if(cart.Count() > 0)
            {
                return RedirectToAction("Index");
            }
            Session["Cart"] = null;
            return RedirectToAction("Index", "Product");
        }
        private decimal SubTotal()
        {
            List<ShoppingCart> cart = GetCart();
            return (decimal)cart.Sum(n => n.Money);
        }
        private int SumAmount()
        {
            List<ShoppingCart> cart = GetCart();
            return (int)cart.Sum(n => n.Amount);
        }
        public ActionResult CartTotal()
        {
            ViewBag.SubTotal = SubTotal();
            ViewBag.Sale = Session["Sale"] as SaleCode;
            return PartialView();
        }
        public ActionResult CheckOut()
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Product");
            }
            if (Session["Customer"] == null)
            {
                TempData["Route"] = "2";
                return RedirectToAction("Login", "Customer");
            }
            ViewBag.CartList = GetCart();
            ViewBag.SubTotal = SubTotal();
            ViewBag.Sale = Session["Sale"] as SaleCode;
            ViewBag.Customer = Session["Customer"] as Customer;
            TempData["Route"] = null;
            return View();
        }
        public ActionResult Shopping()
        {
            ViewBag.SumAmount = 0;
            ViewBag.SubTotal = 0;
            if (Session["Cart"] != null)
            {
                ViewBag.SumAmount = SumAmount();
                ViewBag.SubTotal = SubTotal();
            }
            return PartialView();
        }
        [HttpPost]
        public ActionResult SaleCode(FormCollection frm)
        {
            if(frm["code"].ToString() != String.Empty)
            {
                string s = frm["code"].ToString();
                Session["Sale"] = db.SaleCodes.FirstOrDefault(n => n.Code == s);
            }
            return RedirectToAction("Index");
        }
        public ActionResult PaymentWithVNPAY(decimal amount)
        {
            OrderInvoice bill = new OrderInvoice();
            bill.OrderDate = DateTime.Now;
            bill.DeliveryDate = DateTime.Now;
            Customer customer = Session["Customer"] as Customer;
            bill.CustomerId = customer.Id;
            bill.NameOfConsignee = customer.Lastname + " "  + customer.Firstname;
            bill.AddressOfConsignee = customer.Address;
            bill.PhoneOfConsignee = customer.Phone;
            bill.isDelivered = false;
            bill.isPaid = false;
            bill.Total = amount;
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", Convert.ToString((double)amount * 2300000)); //số tiền cần thanh toán, công thức: số tiền * 2300000 - ví dụ $1 (một đô) --> 23000.00VND
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            string s = DateTime.Now.Ticks.ToString();
            pay.AddRequestData("vnp_TxnRef", s); //mã hóa đơn
            bill.OrderId = s;
            db.OrderInvoices.Add(bill);
            db.SaveChanges();

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }
        public ActionResult PaymentConfirm(int? cash)
        {
            if(cash.HasValue && cash == 1)
            {
                ViewBag.Message = "Order successful";
                return View();
            }
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                OrderInvoice bill = db.OrderInvoices.FirstOrDefault(n => n.OrderId == orderId.ToString());

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        bill.isPaid = true;
                        db.OrderInvoices.AddOrUpdate(bill);
                        db.SaveChanges();
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        Session["Cart"] = null;
                        db.OrderInvoices.Remove(bill);
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    Session["Cart"] = null;
                    db.OrderInvoices.Remove(bill);
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }
        public ActionResult ShipCode(decimal amount)
        {
            OrderInvoice bill = new OrderInvoice();
            bill.OrderId = DateTime.Now.Ticks.ToString();
            Customer customer = Session["Customer"] as Customer;
            bill.CustomerId = customer.Id;
            bill.Total = amount;
            bill.DeliveryDate = DateTime.Now;
            bill.OrderDate = DateTime.Now;
            bill.NameOfConsignee = customer.Lastname + " " + customer.Firstname;
            bill.AddressOfConsignee = customer.Address;
            bill.PhoneOfConsignee = customer.Phone;
            bill.isDelivered = false;
            bill.isPaid = false;
            db.OrderInvoices.Add(bill);
            db.SaveChanges();
            return RedirectToAction("PaymentConfirm", "ShoppingCart", new {cash = 1});
        }
    }
}