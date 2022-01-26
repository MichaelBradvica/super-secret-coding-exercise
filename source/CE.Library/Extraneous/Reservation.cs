using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CE.Library.Extraneous
{
    public class Reservation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime DateTime { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
