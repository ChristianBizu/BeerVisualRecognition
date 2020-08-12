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
using BeerClassifier.Services.Core;
using Microsoft.Extensions.Configuration;

namespace BeerClassWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IIBMVisualRecognitionService _iBMVisualRecognitionService;
        private readonly IConfiguration Configuration;

        private const string CONF_API_KEY = "ApiSettings:ApiKey";
        private const string CONF_API_ENDPOINT = "ApiSettings:ApiEndpoint";
        private const string CONF_API_MODEL_ID = "ApiSettings:ApiModelID";
        private const string CONF_API_OWNER = "ApiSettings:ApiOwner";
        private const string IMAGE_FOLDER = "uploads";

        public HomeController(ILogger<HomeController> logger,
                                IWebHostEnvironment hostingEnvironment,
                                IIBMVisualRecognitionService iBMVisualRecognitionService,
                                IConfiguration configuration)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _iBMVisualRecognitionService = iBMVisualRecognitionService;
            Configuration = configuration;
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
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, IMAGE_FOLDER);
                var filePath = Path.Combine(uploads, uniqueFileName);
                model.MyImage.CopyTo(new FileStream(filePath, FileMode.Create));

                _iBMVisualRecognitionService.ConfigureService(Configuration[CONF_API_KEY], Configuration[CONF_API_ENDPOINT], 
                                                                Configuration[CONF_API_MODEL_ID], Configuration[CONF_API_OWNER]);
            
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ClassifyResponse()
        {
            return View();
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
