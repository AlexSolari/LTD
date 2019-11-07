using Assets.Scripts.Units;
using Assets.Scripts.Units.Enemy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common
{
    public static class Tables
    {
        public static Dictionary<Type, int> TowerPrices = new Dictionary<Type, int>()
        {
            [typeof(Tower)] = 3,
            [typeof(TowerRanged)] = 4
        };

        public static Dictionary<int, Type> WaveEnemies = new Dictionary<int, Type>()
        {
            [1] = typeof(LesserSkeleton),
            [2] = typeof(Wolfrider),
            [3] = typeof(Lich)
        };
    }
}
