using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BeerClassWebApp.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace BeerClassWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger,
                                IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;               
        }

        public IActionResult Index()
        {
            return View( new CreatePost());
        }

        [HttpPost]
        public IActionResult Index(CreatePost model)
        {
            if (model.MyImage != null)
            {
                var uniqueFileName = GetUniqueFileName(model.MyImage.FileName);
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                model.MyImage.CopyTo(new FileStream(filePath, FileMode.Create));

                //to do : Save uniqueFileName  to your db table   
            }
            // to do  : Return something
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AvailableClasses()
        {
            return View();
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

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }

    public class CreatePost
    {
        public string ImageCaption { set; get; }
        public IFormFile MyImage { set; get; }
    }
}
