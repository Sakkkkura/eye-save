using EyeSave.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeSave.ViewModels
{
    public class AgentViewModel : ViewModelBase
    {
        private Agent _agent;
        private List<AgentType> _agentTypes;

        private bool _isNew = false;

        public Agent Agent
        {
            get => _agent;
            set => Set(ref _agent, value, nameof(Agent));
        }

        public List<AgentType> AgentTypes
        {
            get => _agentTypes;
            set => Set(ref _agentTypes, value, nameof(AgentTypes));
        }

        private string _searchValue;

        public string SearchValue
        {
            get => _searchValue;
            set => Set(ref _searchValue, value, nameof(SearchValue));
        }

        private ProductSale _selectedProductSale;

        public ProductSale SelectedProductSale
        {
            get => _selectedProductSale;
            set => Set(ref _selectedProductSale, value, nameof(SelectedProductSale));
        }

        public bool IsNew { get; set; }

        public AgentViewModel(int? agentId)
        {
            using (ApplicationDbContex contex = new())
            {
                AgentTypes = contex.AgentTypes.ToList();
            }
            if (agentId == null)
            {
                IsNew = true;
                Agent = new Agent();
                return;
            }

            Agent = GetAgent((int)agentId);
        }

        public void DeleteSelectedProductSale()
        {
            using (ApplicationDbContex context = new())
            {
                context.ProductSales.Remove(SelectedProductSale);
                context.SaveChanges();
            }
            SelectedProductSale = null;
            Agent = GetAgent(Agent.Id);
        }

        private Agent GetAgent(int agentId)
        {
            using (ApplicationDbContex contex = new())
            {
                return contex.Agents
                   .Include(a => a.AgentType)
                   .Include(a => a.ProductSales)
                   .ThenInclude(s => s.Product)
                   .Single(a => a.Id == agentId);
            }
        }

    }
}
