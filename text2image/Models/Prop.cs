using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRWebSite.Models
{
    public class TextParam
    {
        public string prompt { get; set; }
        public string negative_prompt { get; set; }
       // public string sampler_index { get; set; }
       // public int seed { get; set; }
        public int steps { get; set; }
        public int width { get; set; }
        public int height { get; set; }
       // public int cfg_scale { get; set; }
    }
}