using AspNetCoreSignalR2.Logic;
using AspNetCoreSignalR2.Models;
using Coravel.Queuing.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AspNetCoreSignalR2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQueue _queue;
        private readonly IHubContext<JobProgressHub> _hubContext;

        public HomeController(IQueue queue, IHubContext<JobProgressHub> hubContext)
        {
            _queue = queue;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StartProgress()
        {
            var jobId = Guid.NewGuid().ToString("N");
            _queue.QueueAsyncTask(() => PerformBackgroundJobAsync(jobId));

            return RedirectToAction(nameof(Progress), new { jobId });
        }

        public IActionResult Progress(string jobId)
        {
            ViewBag.JobId = jobId;

            return View();
        }

        private async Task PerformBackgroundJobAsync(string jobId)
        {
            for (int i = 0; i <= 100; i += 5)
            {
                await _hubContext.Clients.Group(jobId).SendAsync("progress", i);

                await Task.Delay(100);
            }
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
