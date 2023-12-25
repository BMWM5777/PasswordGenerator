using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.Password
{
    public class PasswordTemplate
    {
        public string Template { get; set; }
        public string Category { get; set; }
        public bool IncludeUppercase { get; set; }
        public bool IncludeLowercase { get; set; }
        public bool IncludeDigits { get; set; }
        public bool IncludeSpecialCharacters { get; set; }
    }

}
