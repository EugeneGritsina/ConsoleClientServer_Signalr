using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class ChatController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
