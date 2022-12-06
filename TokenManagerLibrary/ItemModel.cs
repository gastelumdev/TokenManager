using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenManagerLibrary
{
    public class ItemModel
    {
        /// <summary>
        /// Represents the name of the item to be sold at event
        /// </summary>
        public string name { get; set; } = string.Empty;
        /// <summary>
        /// Represents the price of the item in tokens
        /// </summary>
        public int price { get; set; } = 0;
    }
}
