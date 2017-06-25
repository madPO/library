
using System.Collections.Generic;

namespace application.Models
{
    public class SearchViewUsers
    {
        public string Name { get; set; }

        public IEnumerable<Users> Users { get; set; }

        public SearchViewUsers()
        {
            Users = new List<Users>();
        }
    }
}