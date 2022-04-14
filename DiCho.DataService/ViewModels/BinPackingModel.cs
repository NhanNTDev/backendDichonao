using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class BinPackingModel
    {
        public double[] Weights { get; set; }
        public int NumItems { get; set; }
        public int NumBins { get; set; }
        public double BinCapacity { get; set; }
    }

    public class TotalBinModel
    {
        public double TotalBin { get; set; }
        public List<BinResultModel> BinResult { get; set; }
    }

    public class BinResultModel
    {
        public double BinNumber { get; set; }
        public BinDetailModel BinDetail { get; set; }
    }

    public class BinDetailModel
    {
        public List<int> Items { get; set; }
        public double TotalWeight { get; set; }
    }
}
