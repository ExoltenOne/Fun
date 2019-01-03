using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LogSpekt.Controllers
{
    public class LogSpektController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowLogGroups()
        {
            return View();
        }
    }
}
