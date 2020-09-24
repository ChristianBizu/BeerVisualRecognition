using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BeerClassifer.WebApp.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using BeerClassifier.Services.Core;
using Microsoft.Extensions.Configuration;
using BeerClassifier.WebApp.Models;
using BeerClassifier.Services.Model;
using MongoRepository;
using BeerClassfier.Entities;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using MvcContrib.Filters;
using BeerClassifier.Services.Infrastructure;

namespace BeerClassWebApp.Controllers
{
    [PassParametersDuringRedirect]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IIBMVisualRecognitionService _iBMVisualRecognitionService;
        private readonly IConfiguration Configuration;
        private readonly IBeerService _beerService;

        private const string CONF_API_KEY = "ApiSettings:ApiKey";
        private const string CONF_API_ENDPOINT = "ApiSettings:ApiEndpoint";
        private const string CONF_API_MODEL_ID = "ApiSettings:ApiModelID";
        private const string CONF_API_OWNER = "ApiSettings:ApiOwner";
        private const string IMAGE_FOLDER = "uploads";
        private const string TEMP_DATA_BEER = "beer";



        public HomeController(ILogger<HomeController> logger,
                                IWebHostEnvironment hostingEnvironment,
                                IIBMVisualRecognitionService iBMVisualRecognitionService,
                                IConfiguration configuration,
                                IBeerService beerService)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _iBMVisualRecognitionService = iBMVisualRecognitionService;
            Configuration = configuration;
            _beerService = beerService;
        }

        public IActionResult Index()
        {
            return View(new CreatePost() { ErrorMessage = "" }); ;
        }


        [HttpPost]
        public IActionResult Index(CreatePost model)
        {
            if (model.MyImage != null)
            {
                var uniqueFileName = GetUniqueFileName(model.MyImage.FileName);
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, IMAGE_FOLDER);
                var filePath = Path.Combine(uploads, uniqueFileName);

                using (var stream = System.IO.File.Open(filePath, FileMode.Create))
                {
                    model.MyImage.CopyTo(stream);
                }

                _iBMVisualRecognitionService.ConfigureService(Configuration[CONF_API_KEY], Configuration[CONF_API_ENDPOINT], 
                                                                Configuration[CONF_API_MODEL_ID], Configuration[CONF_API_OWNER]);

                var response = _iBMVisualRecognitionService.ClassifyImage(model.ImageCaption, filePath);

                if (response.ResponseResult == ResponseResult.OK)
                {
                    var viewModel = CreateModelFromVisualResponse(response);
                    if(viewModel == null || viewModel.Beer == null) return RedirectToAction("Index", "Home");

                    TempData[TEMP_DATA_BEER] = JsonConvert.SerializeObject(viewModel.Beer);
                    return RedirectToAction("ClassifyResponse", viewModel);    
                }
                else 
                {
                    model.ErrorMessage = "Error procesando la imagen";
                    return View(model);
                }            
            }

            return RedirectToAction("Index", "Home");
        }

        private ClassifyResponseViewModel CreateModelFromVisualResponse(IBMClassifyResponseModel model) 
        {
            if (model.Image_procesed_list.Any()
               && model.Image_procesed_list.First().Classifiers_list.Any()
               && model.Image_procesed_list.First().Classifiers_list.First().Classes_list.Any())
            {
                var match = model.Image_procesed_list.First().Classifiers_list.First().Classes_list.First();
                var beer = _beerService.GetBeerByClassName(match.Model_class);

                var beerViewModel = BeerViewModel.FromModel(beer);

                return new ClassifyResponseViewModel() { Accuracy = match.Model_score, Class = match.Model_class, Beer = beerViewModel };
            }

            return null;
        }


        public IActionResult ClassifyResponse(ClassifyResponseViewModel model)
        {
            var viewmodel = JsonConvert.DeserializeObject<BeerViewModel>((string)TempData[TEMP_DATA_BEER]);
            model.Beer = viewmodel;
            return View("ClassifyResponse", model);
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
        public string ErrorMessage { get; set; }
    }
}
