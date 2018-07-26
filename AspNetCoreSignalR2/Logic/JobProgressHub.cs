using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AspNetCoreSignalR2.Logic
{
    public class JobProgressHub : Hub
    {
        public async Task AssociateJobAsync(string jobId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, jobId);
        }
    }
}
