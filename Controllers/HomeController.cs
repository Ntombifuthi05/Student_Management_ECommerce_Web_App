using ASPNETCore_DB.Data;
using ASPNETCore_DB.Interfaces;
using ASPNETCore_DB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;


namespace ASPNETCore_DB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SQLiteDBContext _context;
        private readonly IDBInitializer _seedDatabase;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, SQLiteDBContext context, IDBInitializer seedDatabase, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _seedDatabase = seedDatabase;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            ViewData["UserName"] = _userManager.GetUserName(this.User);

            if (this.User.IsInRole("Admin"))
            {
                ViewData["UserRole"] = "Admin";
            }
            if (this.User.IsInRole("User"))
            {
                ViewData["UserRole"] = "User";
            }
            return View();
        }

        public IActionResult SeedDatabase()
        {
           // _seedDatabase.Initialize(_context);
            ViewBag.SeedDbFeedback = "Database created and Student Table populated with Data. Check Database folder.";
            return View("SeedDatabase");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                var message = new MailMessage();
                message.To.Add("YOUR-EMAIL@example.com"); // Replace with your email
                message.Subject = model.Subject ?? "New Contact Form Submission";
                message.Body = $"Name: {model.Name}\nEmail: {model.Email}\n\nMessage:\n{model.Message}";
                message.From = new MailAddress("YOUR-EMAIL@example.com");

                using (var smtp = new SmtpClient("smtp.gmail.com", 587)) // Use your SMTP settings
                {
                    smtp.Credentials = new NetworkCredential("YOUR-EMAIL@example.com", "YOUR-APP-PASSWORD");
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }

                TempData["SuccessMessage"] = "Your message has been sent successfully!";
                return RedirectToAction("Contact");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(string name, string email, string message)
        {
            var fullMessage = $"From: {name} ({email})\n\nMessage:\n{message}";

            var smtpClient = new SmtpClient("smtp.yourprovider.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("yourEmail@example.com", "yourPassword"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("yourEmail@example.com"),
                Subject = "Website Inquiry",
                Body = fullMessage,
                IsBodyHtml = false,
            };

            mailMessage.To.Add("admin@example.com");

            await smtpClient.SendMailAsync(mailMessage);

            TempData["Message"] = "Email sent successfully!";
            return RedirectToAction("Contact");
        }


    }
}