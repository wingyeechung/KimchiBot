using System;
using System.Collections.Generic;
using System.Text;

namespace KimchiBot
{
    public class UserData
    {
        public string Name { get; set; }
        public int Balance { get; set; }

        public List<string> Food { get; } = new List<string>();
    }
}
