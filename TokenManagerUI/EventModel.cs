using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenManagerUI
{
    internal class EventModel
    {
        /// <summary>
        /// Represents the name of the event
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Represents the price of each token sold at the event
        /// </summary>
        public double TokenPrice { get; set; } = 0.0;
        /// <summary>
        /// Represents a list of items to be sold at the event
        /// </summary>
        public List<ItemModel> Items { get; set; } = new List<ItemModel>();
    }
}
