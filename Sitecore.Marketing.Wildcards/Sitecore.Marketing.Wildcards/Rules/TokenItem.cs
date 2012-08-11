using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Sitecore.Marketing.Wildcards.Rules
{
    public class TokenItem:CustomItem
    {

        public TokenItem(Item innerItem) : base(innerItem)
        {

        }
        public static implicit operator TokenItem(Item item)
        {
            return new TokenItem(item);
        }
        
		public string TokenName
        {
            get { return this.InnerItem.Name; }
        }
        
		public string TokenValue
        {
            get { return this.InnerItem["Value"]; }
        }

		//the bucket of where the item you want it located
		public string DataSource
		{
			get
			{
				return this.InnerItem["DataSource"]; 
			}
		}
    }
}
