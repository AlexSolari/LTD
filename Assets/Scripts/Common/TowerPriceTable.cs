using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common
{
    public static class TowerPriceTable
    {
        public static Dictionary<Type, int> Prices = new Dictionary<Type, int>()
        {
            [typeof(Tower)] = 3,
            [typeof(TowerRanged)] = 4
        };
    }
}
