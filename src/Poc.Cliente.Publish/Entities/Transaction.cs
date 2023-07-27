using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poc.Cliente.Publish.Entities
{
    public class Transaction
    {
        public int document { get; set; }
        public string type { get; set; }
        public string payment { get; set; }
        public float value { get; set; }
        public string date { get; set; }
    }
}