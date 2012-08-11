using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Sitecore.Marketing.Wildcards
{
    public class WildcardInformation
    {
        public Item Item { get; set; }
        public Item WildcardItem { get; set; }


        /// <summary>
        /// There may have been more than one item that matched you can access them here is you want to work it out later
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public IList<Item> Items { get; set; }

        public string TokenValue { get; set; }

        public WildcardInformation()
        {
        }

        public string FullPath { get; set; }

        public string DataSource { get; set; }

        public string Query { get; set; }

        public int TokenIndex { get; set; }

        public bool ContextSwitched { get; set; }
    }
}
