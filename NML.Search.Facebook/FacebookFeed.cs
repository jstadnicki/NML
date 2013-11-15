using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NML.Search.Facebook
{
    public class FacebookFeed
    {
        public string Id { get; set; }
        public FacebookUser From { get; set; }
        public string Picture { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
    }
}
