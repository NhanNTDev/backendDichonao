using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{


    public partial class Rootobject
    {
        public string type { get; set; }
        public Feature[] features { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

    public partial class Geometry
    {
        public string type { get; set; }
        public decimal[][][] coordinates { get; set; }
    }

    public class Properties
    {
        [BindNever]
        public string f1 { get; set; }
        [BindNever]
        public string f2 { get; set; }
        [BindNever]
        public int f3 { get; set; }
        [BindNever]
        public int f4 { get; set; }
    }

    public class Zones
    {
        [BindNever]
        public int Id { get; set; }
        [BindNever]
        public string Name { get; set; }
    }
}
