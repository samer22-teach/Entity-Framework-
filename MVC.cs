using System;
using System.Linq;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // 1. Existing Create GET method
        public ActionResult Create()
        {
            return View();
        }

        // 2. Updated Create POST method with your logic
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                decimal monthlyQuote = 50;

                int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
                if (insuree.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

                if (age <= 18) monthlyQuote += 100;
                else if (age >= 19 && age <= 25) monthlyQuote += 50;
                else monthlyQuote += 25;

                if (insuree.CarYear < 2000) monthlyQuote += 25;
                if (insuree.CarYear > 2015) monthlyQuote += 25;

                if (insuree.CarMake.ToLower() == "porsche")
                {
                    monthlyQuote += 25;
                    if (insuree.CarModel.ToLower() == "911 carrera") monthlyQuote += 25;
                }

                monthlyQuote += (insuree.SpeedingTickets * 10);
                if (insuree.DUI) monthlyQuote *= 1.25m;
                if (insuree.CoverageType) monthlyQuote *= 1.50m;

                insuree.Quote = monthlyQuote;

                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // 3. New Admin Action
        public ActionResult Admin()
        {
            return View(db.Insurees.ToList());
        }
    }
}

