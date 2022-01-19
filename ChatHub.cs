using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Chat.Models;
using System.Linq;

namespace Chat
{
    
    [Authorize]
    public class ChatHub : Hub
    {
        private string KeyWord = "зачет по программированию";

        private System.IServiceProvider _sp;
        
        public ChatHub(System.IServiceProvider sp)
        {
            _sp = sp;
        }

        public async Task Send(string message, string to)
        {
            var userName = Context.User.Identity.Name;
            bool IsUserKnow = await IsUserKnowNewsPolicarp(userName);
            

            if (!IsUserKnow && !userName.Contains("policarp")) return;

            using (var scope = _sp.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var messages = await _context.Rules.ToListAsync();

                var users = await _context.Users.ToListAsync();
                var user = users.Where(x => x.Email.Equals(userName)).ToList();
                int quatity_messages = messages.Where(x => x.User.Equals(userName)).Count();
                if (user.First().MQM < quatity_messages) return; 
  
                Rule rule = new Rule { User = userName, Message = message, To = to };
                _context.Rules.Add(rule);
                await _context.SaveChangesAsync();
            }

            

            if (Context.UserIdentifier != to) // если получатель и текущий пользователь не совпадают
                await Clients.User(Context.UserIdentifier).SendAsync("Receive", message, userName);
            await Clients.User(to).SendAsync("Receive", message, userName);

            bool AllKnowsFlag = await IsAllUsersKnowNewsPlicarp();
            if (AllKnowsFlag) await OnAllKnow();
        }

        private async Task<bool> IsUserKnowNewsPolicarp(string userName)
        {
            var currentUserName = Context.User.Identity.Name;

            using (var scope = _sp.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var messages = await _context.Rules.ToListAsync();
                
                int quatity_messages = 
                    messages.Where(
                        x => x.Message.Contains(KeyWord) &&
                        x.To.Equals(userName)).Count();

                return (quatity_messages > 0) ? true : false;
            }
        }

        private async Task<bool> IsAllUsersKnowNewsPlicarp()
        {
            using (var scope = _sp.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var messages = await _context.Rules.ToListAsync();
                var users = await _context.Users.ToListAsync();

                var ThoseWhoKnowNews =
                    messages.Where(x => x.Message.Contains(KeyWord));

                int knowsUsers = ThoseWhoKnowNews.Select(x => x.To).Distinct().Count();
                int allUsers = users.Count();

                return (knowsUsers + 1 == allUsers) ? true : false;
            }
        }


        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", $"Приветствуем {Context.UserIdentifier}");
            await base.OnConnectedAsync();
        }


        public async Task OnAllKnow()
        {
            await Clients.All.SendAsync("Notify", $"Все предупреждены!");
            await base.OnConnectedAsync();
        }

    }
}