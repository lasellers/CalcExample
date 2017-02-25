using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calc.Models
{
    public class CalcStorageFormat
    {
        public int sum;
        public List<string> history;
    }

    public class CalcValidate
    {

        public static Boolean ValidateValueIsNumeric(string suspectNumber)
        {
            string pattern = @"^-?\d+(,\d+)*(\.\d+(e\d+)?)?$";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(suspectNumber);

#if DEBUG
            if (matches.Count > 0)
            {
                Debug.WriteLine("{0} ({1} matches):", suspectNumber, matches.Count);
                foreach (Match match in matches)
                    Debug.WriteLine("   " + match.Value);
            }
            else
            {
                Debug.WriteLine("No Matches");
            }
#endif

            return (matches.Count > 0);
        }

    }

}
