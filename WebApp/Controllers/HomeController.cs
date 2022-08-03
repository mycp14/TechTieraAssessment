using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WebApp.Data.ViewModel;
using WebApp.Models;
using WebApp.Service.IService;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITransactionService _transactionService;
        public HomeController(ILogger<HomeController> logger, ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        public IActionResult Index()
        {
            TempData["ErrorMessage"] = "";
            return View();
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> Index(IFormFile files)
        {
            var supportedTypes = new[] { "csv", "xml"};
            var fileExt = Path.GetExtension(files.FileName).Substring(1);
            if (!supportedTypes.Contains(fileExt.ToLower()))
            {
                TempData["ErrorMessage"] = "Unknown format!";
                return View();
            }
            var list = new List<UploadTransactionViewModel>();
            //less than or equal to 1MB only
            if (files.Length <= 1000000)
            {
                var vm = new UploadTransactionViewModel();
                if (files.FileName.EndsWith(".csv"))
                {
                    using (var sreader = new StreamReader(files.OpenReadStream()))
                    {
                        string[] headers = sreader.ReadLine().Split(',');     //Title
                        while (!sreader.EndOfStream)                          //get all the content in rows 
                        {
                            string[] rows = sreader.ReadLine().Split(',');
                            vm.TransactionId = rows[0].ToString().TrimStart();
                            vm.Amount = rows[1].ToString().TrimStart();
                            vm.CurrencyCode = rows[2].ToString().TrimStart();
                            vm.TransactionDate = rows[3].ToString().TrimStart(); //DateTime.Parse(rows[3].ToString().TrimStart());
                            vm.Status = rows[4].ToString().TrimStart();
                            var errorMessage = new List<string>();
                            if (!_transactionService.ValidateFields(vm, out errorMessage))
                            {
                                string message = "";
                                foreach(var error in errorMessage)
                                {
                                    message = $"{message} {Environment.NewLine} {error}. ";
                                }
                                if (!string.IsNullOrEmpty(message))
                                {
                                    TempData["ErrorMessage"] = message;
                                    return View();
                                }
                            }
                            list.Add(vm);
                        }
                    }
                }
                else if (files.FileName.EndsWith(".xml"))
                {
                    try
                    {
                        var serializer = new XmlSerializer(typeof(Transactions));
                        var transactionResult = (Transactions) serializer.Deserialize(files.OpenReadStream());
                        foreach (var data in transactionResult.Transaction)
                        {
                            vm.TransactionId = data.TransactionId;
                            vm.TransactionDate = data.TransactionDate;
                            vm.Status = data.Status;
                            vm.Amount = data.PaymentDetails.Amount;
                            vm.CurrencyCode = data.PaymentDetails.CurrencyCode;
                            var errorMessage = new List<string>();
                            if (!_transactionService.ValidateFields(vm, out errorMessage))
                            {
                                string message = "";
                                foreach (var error in errorMessage)
                                {
                                    message = $"{message} {Environment.NewLine} {error}. ";
                                }
                                if (!string.IsNullOrEmpty(message))
                                {
                                    TempData["ErrorMessage"] = message;
                                    return View();
                                }
                            }
                            list.Add(vm);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.GetBaseException());
                    }

                }
            }
            else
            {
                TempData["ErrorMessage"] = "File size is max 1 MB!";
                return View();
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            TempData["ErrorMessage"] = "Upload Success!";
            _transactionService.Save(list);
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
    }
}
