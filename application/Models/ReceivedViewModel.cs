using System.Collections.Generic;

namespace application.Models
{
    public class ReceivedViewModel
    {
        public Books Book { get; set; }
        public string Name { get; set; }
        public IList<Users> Users {get; set;}
        public int Count { get; set; }

        public ReceivedViewModel()
        {
            Users = new List<Users>();
        }
    }
}