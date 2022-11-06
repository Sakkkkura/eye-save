using EyeSave.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeSave.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public List<Agent> Agents { get; set; }
        private List<Agent> _displayingAgents;

        

        private Agent _selectedAgent;
        private string _selectedSort;
        private string _selectedFilter;
        private string _searchingString;

        



        public List<Agent> DisplayingAgents
        {
            get => _displayingAgents;
            set => Set(ref _displayingAgents, value, nameof(DisplayingAgents));
        }

        public Agent SelectedAgent
        {
            get => _selectedAgent;
            set => Set(ref _selectedAgent, value, nameof(SelectedAgent));
        }  
        public string SelectedSort
        {
            get => _selectedSort;
            set
            {
                Set(ref _selectedSort, value, nameof(SelectedSort));
                DisplayAgents();
            }
        } 
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                Set(ref _selectedFilter, value, nameof(SelectedFilter));
                DisplayAgents();
            }
        }
        public string SearchingString
        {
            get => _searchingString;
            set
            {
                Set(ref _searchingString, value, nameof(SearchingString));
                DisplayAgents();
            }
        }


        public List<string> SortList { get; } = new List<string>
        {
            "Без сортировки",
            "Наименование (возр)",
            "Наименование (уб)",
            "Размер скидки (возр)",
            "Размер скидки (уб)",
            "Приоритет (возр)",
            "Приоритет (уб)"
        };

        public List<string> FilterList { get; } = new List<string>
        {
            "Все типы"
        };

        public MainWindowViewModel()
        {
            using(ApplicationDbContex context = new ApplicationDbContex())
            {
                Agents = context.Agents.AsNoTracking()
                    .Include(a => a.AgentType)
                    .Include(a => a.ProductSales)
                    .ThenInclude(ps => ps.Product)
                    .OrderBy(a => a.Id)
                    .ToList();

                FilterList.AddRange(context.AgentTypes.Select(at => at.Title));
            }
            _displayingAgents = Agents;
        }

        private void DisplayAgents()
        {
            DisplayingAgents = Sort(Search(Filter(Agents)));
        }


        private List<Agent> Sort(List<Agent> incList)
        {
            if (SelectedSort == SortList[1])
                return incList.OrderBy(a => a.Title).ToList();
            else if (SelectedSort == SortList[2])
                return incList.OrderByDescending(a => a.Title).ToList();
            if (SelectedSort == SortList[3])
                return incList.OrderBy(a => a.Discount).ToList();
            else if (SelectedSort == SortList[4])
                return incList.OrderByDescending(a => a.Discount).ToList();
            if (SelectedSort == SortList[5])
                return incList.OrderBy(a => a.Priority).ToList();
            else if (SelectedSort == SortList[6])
                return incList.OrderByDescending(a => a.Priority).ToList();
            else return incList;
        }

        private List<Agent> Search(List<Agent> incList)
        {
            if ((SearchingString == String.Empty) || (SearchingString == null))
                return incList;

            return incList.Where(a => a.Title.Contains(SearchingString)).ToList();
        }

        private List<Agent> Filter(List<Agent> incList)
        {
            if (SelectedFilter == FilterList[0])
                return incList;

            else
                return incList.Where(a => a.AgentType.Title == SelectedFilter).ToList();

            
        }




            



    }
}
