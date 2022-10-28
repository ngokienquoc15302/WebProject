using Microsoft.Ajax.Utilities;
using SpiderFoodStore.Data;
using SpiderFoodStore.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;

namespace SpiderFoodStore.Areas.QWRtaW5TcGlkZXI.Controllers
{
    public class ChartsController : Controller
    {
        SpiderFoodStoreContext db = new SpiderFoodStoreContext();
        private void fill_n(int[] arr, int n)
        {
            for(int i = 0; i < n; i++)
            {
                arr[i] = 0;
            }
        }
        private void refill_n(int[] arr, List<PairI> list) 
        { 
            foreach(PairI item in list)
            {
                arr[(int)item.first - 1] = item.second;
            }
        }
        private List<PairI> GetMonth()
        {
            int yearI = DateTime.Now.Year;
            return (from OrderInvoices in db.OrderInvoices
                    where
                      OrderInvoices.OrderDate.Year == yearI
                    group OrderInvoices by new
                    {
                        Column1 = (int)OrderInvoices.OrderDate.Month
                    } into g
                    select new PairI
                    {
                        first = g.Key.Column1,
                        second = g.Count()
                    }).ToList();
        }
        private List<PairI> GetDay()
        {
            int monthI = DateTime.Now.Month;
            int yearI = DateTime.Now.Year;
            return (from OrderInvoices in db.OrderInvoices
                    where
                      OrderInvoices.OrderDate.Year == yearI &&
                      OrderInvoices.OrderDate.Month == monthI
                    group OrderInvoices by new
                    {
                        Column1 = (int)OrderInvoices.OrderDate.Day
                    } into g
                    select new PairI
                    {
                        first = g.Key.Column1,
                        second = g.Count()
                    }).ToList();
        }
        // GET: QWRtaW5TcGlkZXI/Charts
        public ActionResult Index()
        {
            List<PairI> list = GetMonth();
            int[] listM = new int[12];
            fill_n(listM, 12);
            refill_n(listM, list);
            ViewBag.Month = listM;
            list = GetDay();
            int numberOfDayMount = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            int[] listD = new int[numberOfDayMount];
            fill_n(listD, numberOfDayMount);
            refill_n(listD, list);
            ViewBag.Day = listD;
            ViewBag.DayNo = numberOfDayMount;
            return View();
        }
    }
}