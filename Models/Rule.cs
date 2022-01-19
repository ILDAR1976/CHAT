using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class Rule
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
    }
}
