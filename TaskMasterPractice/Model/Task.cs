using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMasterPractice.Model
{
    internal class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // make it nullable
        public DateTime? DueDate { get; set; }
        public int StatusId { get; set; }
        // navigation property pointing to Status class
        public Status Status { get; set; }
    }
}
