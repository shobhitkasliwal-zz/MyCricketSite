using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCricketSiteData.Entities
{
    public class User : MongoEntity
    {
        public User() { }

        public string Issuer { get; set; }
        public string ProviderKey { get; set; }
        public string Name { get; set; }
        public string emailAddress { get; set; }
        public string ImageUrl { get; set; }

    }
}
