using DocumentsWebsite.Hubs;
using DocumentsWebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DocumentsWebsite.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ILogger<DocumentsController> _logger;
        private readonly IHubContext<DocumentsHub> _documentsHubContext;

        public DocumentsController(ILogger<DocumentsController> logger, IHubContext<DocumentsHub> documentsHubContext)
        {
            _documentsHubContext = documentsHubContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] string clientId)
        {
            var documentsViewModel = new DocumentsViewModel
            {
                ClientId = clientId,
            };

            return View(documentsViewModel);
        }

        [HttpGet("documents/printdocument/{clientId}")]
        public async Task<IActionResult> PrintDocument([FromRoute] string clientId)
        {
            await _documentsHubContext.Clients.Group(clientId).SendAsync("printDocument", "This is document content");
            return Ok();
        }
    }
}
