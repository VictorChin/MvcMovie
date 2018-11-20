using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.ViewComponents
{
    public class demoViewComponent : ViewComponent
    {
        private MvcMovieContext _context;

        public demoViewComponent(MvcMovieContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View("_demoPartial", new DemoViewModel { MovieCount=_context.Movie.Count() });
        }
    }

    public class DemoViewModel { public int MovieCount; }
}
