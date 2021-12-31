using Demo_NotificationFromServerToClient_SignalR.Hubs;
using Demo_NotificationFromServerToClient_SignalR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private IHubContext<NotificationProcessHub> notificationProcessHub;

        public HomeController(ILogger<HomeController> logger, IHubContext<NotificationProcessHub> notificationProcessHub)
        {
            _logger = logger;
            this.notificationProcessHub = notificationProcessHub;
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
        public IActionResult Process(ProcessExecution processExecution)
        {
            Task t = Task.Run(async () => await RunProcess(notificationProcessHub, processExecution));

            _logger.LogInformation($"Se lanzó el Proceso {processExecution.ProcessName} - {DateTime.Now}");
            return Json(processExecution.ProcessName);
        }

        private async Task RunProcess(IHubContext<NotificationProcessHub> hub, ProcessExecution process)
        {
            _logger.LogInformation($"Proceso {process.ProcessName} iniciado - {DateTime.Now}");
            await Task.Delay(process.ProcessDuration * 1000);
            _logger.LogInformation($"Proceso {process.ProcessName} terminado - {DateTime.Now}");
            await hub.Clients.All.SendAsync("ReceiveProcessNotification", $"Proceso {process.ProcessName} terminado", 10);
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
