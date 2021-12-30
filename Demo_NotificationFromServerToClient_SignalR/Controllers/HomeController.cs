using Demo_NotificationFromServerToClient_SignalR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_NotificationFromServerToClient_SignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Process()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Process(ProcessExecution processExecution)
        {
            Task t = Task.Run(async () =>
            {
                _logger.LogInformation($"Proceso {processExecution.ProcessName} iniciado - {DateTime.Now}");
                await Task.Delay(processExecution.ProcessDuration * 1000);
                _logger.LogInformation($"Proceso {processExecution.ProcessName} terminado - {DateTime.Now}");
                //aca se llamaria el hub para notificar
            });

            _logger.LogInformation($"Se lanzó el Proceso {processExecution.ProcessName} - {DateTime.Now}");
            return Json(processExecution.ProcessName);
        }

        public IActionResult Notification()
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
    }
}
