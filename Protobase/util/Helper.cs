using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protobase.util
{
    public static class GuidHelper
    {
        private static HashSet<string> guids = new HashSet<string>();
        public static Guid GetUniqueGuid()
        {
            Guid result = Guid.NewGuid();
            string g = result.ToString();
            if (guids.Contains(g))
            {
                return GetUniqueGuid();
            }

            guids.Add(g);
            return result;
        }
    }
}
