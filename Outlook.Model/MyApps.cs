using System.Collections.Generic;

namespace OutlookIndia.Model
{
    public class MyApp
    {
        public string title { get; set; }

        public string imageUrl { get; set; }

        public string description { get; set; }

        public string appUrl { get; set; }

        public string containerID { get; set; }
    }

    public class MyApps
    {
        public List<MyApp> myapps { get; set; }
    }
}