using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
//Group NAme: Sharp Future
//223001495 MAKOSONKE NP
//223074090 LIPHAPANG S
//223050534 BLOU A
//223056856 RAMMUTLA OR
//223089666 Pico MNP
//223058521 SHUPING KO

using ASPNETCore_DB.Models;
using ASPNETCore_DB.Repositories;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

   

    [HttpPost]
    public IActionResult Send(string name, string email, string message)
    {
        try
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("picomoemedi5@gmail.com", "P@ss223344"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(email),
                Subject = "Website Inquiry",
                Body = $"Name: {name}\nEmail: {email}\nMessage:\n{message}",
                IsBodyHtml = false,
            };
            mailMessage.To.Add("picomoemedi5@gmail.com");

            smtpClient.Send(mailMessage);

            ViewBag.Message = "Email sent successfully!";
        }
        catch (Exception ex)
        {
            ViewBag.Message = $"Error sending email: {ex.Message}";
        }

        return View("Index");
    }
}

