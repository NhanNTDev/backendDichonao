using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.Core.Custom
{
    public class PropertyClass
    {
        public Type GetType(string str)
        {
            return Type.GetType("DiCho.DataService.Models." + str);
        }
    }
}
