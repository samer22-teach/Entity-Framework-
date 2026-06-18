[HttpPost]

[ValidateAntiForgeryToken]

public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType")] Insuree insuree)

{

    if (ModelState.IsValid)

    {

        decimal monthlyQuote = 50;

 

        // Age Logic

        int age = DateTime.Today.Year - insuree.DateOfBirth.Year;

        if (insuree.DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;

 

        if (age <= 18) monthlyQuote += 100;

        else if (age <= 25) monthlyQuote += 50;

        else monthlyQuote += 25;

 

        // Car Year Logic

        if (insuree.CarYear < 2000) monthlyQuote += 25;

        if (insuree.CarYear > 2015) monthlyQuote += 25;

 

        // Porsche Logic

        if (insuree.CarMake.ToLower() == "porsche")

        {

            monthlyQuote += 25;

            if (insuree.CarModel.ToLower() == "911 carrera") monthlyQuote += 25;

        }

 

        // Speeding Tickets

        monthlyQuote += (insuree.SpeedingTickets * 10);

 

        // Percentage Modifiers (DUI and Coverage)

        if (insuree.DUI) monthlyQuote *= 1.25m;

        if (insuree.CoverageType) monthlyQuote *= 1.50m;

 

        insuree.Quote = monthlyQuote;

 

        db.Insurees.Add(insuree);

        db.SaveChanges();

        return RedirectToAction("Index");

    }

    return View(insuree);

}

@Html.HiddenFor(model => model.Quote, new { Value = 0 })

@model IEnumerable<CarInsurance.Models.Insuree>

<h2>Admin Dashboard: All Quotes</h2>

<table class="table">

    <tr>

        <th>First Name</th>

        <th>Last Name</th>

        <th>Email Address</th>

        <th>Quote</th>

    </tr>

    @foreach (var item in Model) {

    <tr>

        <td>@item.FirstName</td>

        <td>@item.LastName</td>

        <td>@item.EmailAddress</td>

        <td>@item.Quote.ToString("C")</td>

    </tr>

    }

</table>

git init

git add .

git commit -m "Final submission with quote logic and admin view"

git remote add origin <YOUR_GITHUB_URL>

git push -u origin main

 