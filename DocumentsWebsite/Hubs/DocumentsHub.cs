using Microsoft.AspNetCore.SignalR;

namespace DocumentsWebsite.Hubs
{
    public class DocumentsHub : Hub
    {
        private readonly ILogger<DocumentsHub> _logger;

        public DocumentsHub(ILogger<DocumentsHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var httpContext = Context.GetHttpContext();
                var clientId = httpContext?.Request.Query["ClientId"].ToString().ToLowerInvariant(); ;
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{clientId}");
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var httpContext = Context.GetHttpContext();

                string? clientId = httpContext?.Request.Query["ClientId"]
                    .ToString()
                    .ToLowerInvariant();

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{clientId}");
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
