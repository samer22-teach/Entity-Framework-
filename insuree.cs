using System;

public class Class1
{
	public Class1()
	{
	}
}
public class Insuree
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int CarYear { get; set; }
    public string CarMake { get; set; }
    public string CarModel { get; set; }
    public bool DUI { get; set; }
    public int SpeedingTickets { get; set; }
    public bool CoverageType { get; set; } // true = Full, false = Liability
    public decimal Quote { get; set; }
}
using System.Data.Entity;

public class InsuranceContext : DbContext
{
    public DbSet<Insuree> Insurees { get; set; }
}
static void Main(string[] args)
{
    using (var db = new InsuranceContext())
    {
        var newInsuree = new Insuree();

        Console.WriteLine("Enter First Name:");
        newInsuree.FirstName = Console.ReadLine();

        // ... Continue collecting all other fields (CarYear, DUI, etc.) ...

        // Insert your calculation logic here (exactly as you wrote it previously)
        decimal baseQuote = 50m;
        // ... calculation steps ...
        newInsuree.Quote = baseQuote;

        // Save to Database
        db.Insurees.Add(newInsuree);
        db.SaveChanges();

        Console.WriteLine($"Quote generated successfully! Total: {newInsuree.Quote:C}");
    }
}

```csharp
public class InsureeController : Controller
{
    private ApplicationDbContext db = new ApplicationDbContext();

    // Your existing action methods...

    public ActionResult Create(Insuree insuree)
    {
        if (ModelState.IsValid)
        {
            insuree.Quote = CalculateQuote(insuree);
            db.Insurees.Add(insuree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(insuree);
    }

    private decimal CalculateQuote(Insuree insuree)
    {
        decimal quote = 50; // base price

        // Age calculations
        if (insuree.Age <= 18)
        {
            quote += 100;
        }
        else if (insuree.Age >= 19 && insuree.Age <= 25)
        {
            quote += 50;
        }
        else if (insuree.Age >= 26)
        {
            quote += 25;
        }

        // Car year calculations
        if (insuree.CarYear < 2000)
        {
            quote += 25;
        }
        else if (insuree.CarYear > 2015)
        {
            quote += 25;
        }

        // Car make calculations
        if (insuree.CarMake == "Porsche")
        {
            quote += 25; // Base Porsche fee
            if (insuree.CarModel == "911 Carrera")
            {
                quote += 25; // Additional fee for this model
            }
        }

        // Speeding tickets
        quote += insuree.SpeedingTickets * 10;

        // DUI check
        if (insuree.HasDUI)
        {
            quote *= 1.25m; // Add 25%
        }

        // Full coverage check
        if (insuree.IsFullCoverage)
        {
            quote *= 1.50m; // Add 50%
        }

        return quote;
    }
}
```

```html
@model YourNamespace.Models.Insuree

@using (Html.BeginForm())
{
    @Html.AntiForgeryToken()
    
    <div class="form-group">
        @Html.LabelFor(m => m.FirstName)
        @Html.TextBoxFor(m => m.FirstName, new { @class = "form-control" })
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.LastName)
        @Html.TextBoxFor(m => m.LastName, new { @class = "form-control" })
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.Email)
        @Html.TextBoxFor(m => m.Email, new { @class = "form-control" })
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.Age)
        @Html.TextBoxFor(m => m.Age, new { @class = "form-control" })
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.CarYear)
        @Html.TextBoxFor(m => m.CarYear, new { @class = "form-control" })
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.CarMake)
        @Html.TextBoxFor(m => m.CarMake, new { @class = "form-control" })
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.CarModel)
        @Html.TextBoxFor(m => m.CarModel, new { @class = "form-control" })
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.SpeedingTickets)
        @Html.TextBoxFor(m => m.SpeedingTickets, new { @class = "form-control" })
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.HasDUI)
        @Html.CheckBoxFor(m => m.HasDUI)
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.IsFullCoverage)
        @Html.CheckBoxFor(m => m.IsFullCoverage)
    </div>
    
    <button type="submit" class="btn btn-primary">Submit</button>
}
```
```html
@model IEnumerable<YourNamespace.Models.Insuree>

<h2>Admin View</h2>

<table class="table">
    <thead>
        <tr>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Email</th>
            <th>Quote</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var insuree in Model)
        {
            <tr>
                <td>@insuree.FirstName</td>
                <td>@insuree.LastName</td>
                <td>@insuree.Email</td>
                <td>@insuree.Quote</td>
            </tr>
        }
    </tbody>
</table>
```

```csharp
public ActionResult Admin()
{
    var insurees = db.Insurees.ToList();
    return View(insurees);
}
```


