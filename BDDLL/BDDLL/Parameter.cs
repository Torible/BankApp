using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDLL
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Parameter(string name, object value)
        {
            // Избавиться от всех лишних "@" в строке.
            name = name.Replace("@", "");
            Name = $"@{name}";
            Value = value.ToString();
        }
    }
}