using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NML.Search.Facebook
{
    public class FacebookUser
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Url { get { return String.Format("http://www.facebook.com/profile.php?id={0}", Id); } }
        public string Picture { get { return String.Format("http://graph.facebook.com/{0}/picture?type=square", Id); } }
    
    }
}
