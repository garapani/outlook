using Outlook.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookIndia.Model
{
    public class Category
    {
        [PrimaryKey]
        public string CategoryName { get; set; }
        public string CategoryUrl { get; set; }
        public bool CanShow { get; set; }
        public bool isLoading;
        public bool isRrefreshing;
        public ObservableCollection<Article> Articles;
        public DateTime LastUpdated { get; set; }
    }
}
