using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph
{
    public static class IdentifierExtensions
    {
        public static string ToGraphId(this string s)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
        }

        public static string ToGraphId(this int i)
        {
            return ToGraphId(i.ToString());
        }

        public static string DecodeGraphId(this string s)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(s));
        }

        public static int DecodeGraphIdInt(this string s)
        {
            return int.Parse(DecodeGraphId(s));
        }
    }
}
