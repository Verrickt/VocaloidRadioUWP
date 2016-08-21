using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocaloidRadioShared
{
    public static class EnumHelper
    {
        public static T Parse<T>(string value)
        {
            return (T) System.Enum.Parse(typeof(T), value);
        }
    }
}
