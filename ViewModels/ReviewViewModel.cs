using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1_.NET.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public bool Important { get; set; }

        public DateTime DateTime { get; set; }
    }
}
