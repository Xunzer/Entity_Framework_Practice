using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMasterPractice.Model
{
    class Status
    {
        // naming convention so that Id would be treated as primary key later on for migration
        public int Id { get; set; }
        public string Name { get; set; }

        // override the ToString() method so that inside the combo box would show the name of the types only
        public override string ToString()
        {
            return Name;
        }
    }
}
