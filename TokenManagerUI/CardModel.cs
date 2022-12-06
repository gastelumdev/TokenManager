using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenManagerUI
{
    public class CardModel
    {
        /// <summary>
        /// Represents a unique number of the card
        /// </summary>
        public int CardNumber { get; set; } = 0;
        /// <summary>
        /// Represents amount of tokens in the card
        /// </summary>
        public int TokenAmount { get; set; } = 0;

    }
}
