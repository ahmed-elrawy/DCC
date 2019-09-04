using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCC.API.Model
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public string Descraption { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }

    }
}
