using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using TesteRotativa.Models;
using TesteRotativa.Helpers;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Html2pdf;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Net.Mime;

namespace TesteRotativa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConverter _converter;
        private readonly IViewRenderService _viewRenderService;
        private string BaseHref => $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        public HomeController(ILogger<HomeController> logger, IConverter converter, IViewRenderService viewRenderService)
        {
            _converter = converter;
            _logger = logger;
            _viewRenderService = viewRenderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult RotativaTest()
        {
            ViewData["Title"] = "Título da Página.";
            var model = new TestModel { Name = "Fulano de Tal" };
            return new ViewAsPdf(model, ViewData);
        }
        
        public async Task<FileResult> ItextTest()
        {
            var model = new TestModel { Name = "Fulano de Tal" };
            var html = await _viewRenderService.RenderToStringAsync("Home//ItextTest", model);
            ConverterProperties converterProperties = new();
            converterProperties.SetBaseUri(BaseHref);
            using var stream = new MemoryStream();
            HtmlConverter.ConvertToPdf(html, stream, converterProperties);
            return File(stream.ToArray(), MediaTypeNames.Application.Pdf, "TesteItext.pdf");
        }






    }
}