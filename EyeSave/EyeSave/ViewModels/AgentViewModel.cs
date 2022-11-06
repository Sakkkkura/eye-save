using EyeSave.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeSave.ViewModels
{
    public class AgentViewModel : ViewModelBase
    {
        public Agent _agent;

        public Agent Agent
        {
            get => _agent;
            set => Set(ref _agent, value, nameof(Agent));
        }

        public bool IsNew { get; set; }

        public AgentViewModel(int? agentId)
        {
            if (agentId == null)
            {
                IsNew = true;
                Agent = new Agent();
                return;
            }

            using (ApplicationDbContex contex = new())
            {
                Agent = contex.Agents
                   .Include(a => a.AgentType)
                   .Include(a => a.ProductSales)
                   .ThenInclude(s => s.Product)
                   .Single(a => a.Id == agentId);
            }
            
        }
    }
}
