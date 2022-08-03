using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
                            if (_transactionService.isEmptyField(rows[0].ToString()) || _transactionService.isEmptyField(rows[1].ToString()) 
                                || _transactionService.isEmptyField(rows[2].ToString()) || _transactionService.isEmptyField(rows[3].ToString()) 
                                || _transactionService.isEmptyField(rows[4].ToString()) || rows[0].ToString().Length > 50)
                            {
                                TempData["ErrorMessage"] = "All fields are required!";
                                return View();
                            }
                            vm.TransactionId = rows[0].ToString().TrimStart();
                            decimal decimalVal = 0;
                            try
                            {
                                decimalVal = Convert.ToDecimal(rows[1].ToString().TrimStart());
                                vm.Amount = decimalVal;
                            }
                            catch (FormatException)
                            {
                                TempData["ErrorMessage"] = "The amount is not formatted as a decimal.";
                                return View();
                            }

                            if (!_transactionService.IsValidCurrency(rows[2].ToString().TrimStart()))
                            {
                                TempData["ErrorMessage"] = "Invalid currency code";
                                return View();
                            }
                            vm.CurrencyCode = rows[2].ToString().TrimStart();
                            vm.TransactionDate = DateTime.Parse(rows[3].ToString().TrimStart());
                            if (_transactionService.IsValidStatus(rows[4].ToString().TrimStart()))
                            {
                                TempData["ErrorMessage"] = "Invalid Status";
                                return View();
                            }
                            vm.Status = rows[4].ToString().TrimStart();

                            _transactionService.Save(vm);
                        }
                    }
                }
                else if (files.FileName.EndsWith(".xml"))
                {
                    try
                    {
                        var serializer = new XmlSerializer(typeof(TransactionXMLViewModel));
                        var transactionResult = serializer.Deserialize(files.OpenReadStream());

                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.GetBaseException());
                    }
                    //using (var ms = new MemoryStream())
                    //{
                    //    files.CopyTo(ms);
                    //    var fileBytes = ms.ToArray();
                    //    string s = Convert.ToBase64String(fileBytes);
                    //    // act on the Base64 data
                    //    using (XmlReader reader = XmlReader.Create(ms))
                    //    {
                    //        while (reader.Read())
                    //        {
                    //            if (reader.IsStartElement())
                    //            {
                    //                //return only when you have START tag  
                    //                switch (reader.NodeType)
                    //                {
                    //                    case XmlNodeType.Element:
                    //                        Console.WriteLine("Name of the Element is : " + reader.Name);
                    //                        break;
                    //                    //case "TransactionDate":
                    //                    //    Console.WriteLine("Your Location is : " + reader.ReadString());
                    //                    //    break;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

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
