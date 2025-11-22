using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TidesAndSunrise.Enums
{
    public enum Village
    {
        Bethel,
        Quinhagak,
        Togiak
    }

    public static class VillageExtensions
    {
        public static string ToShortString(this Village village)
        {
            return village switch
            {
                Village.Bethel => "Bethel",
                Village.Quinhagak => "Quinhagak",
                Village.Togiak => "Togiak at Black Rock",
                _ => throw new ArgumentOutOfRangeException(nameof(village), village, null)
            };
        }
    }
}
