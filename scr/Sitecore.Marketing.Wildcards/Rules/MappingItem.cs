using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Marketing.Wildcards.Rules
{
    public class MappingItem:CustomItem
    {
        public MappingItem(Item innerItem) : base(innerItem)
        {
        }
        public static implicit operator MappingItem(Item item)
        {
            return new MappingItem(item);
        }

        public int Position
        {
            get
            {
                var position = 0;
                int.TryParse(this.InnerItem["Position"], out position);
                return position;
            }
        }

        private TokenItem _tokenItem = null;
        public TokenItem TokenItem
        {
            get 
            { 
                if (_tokenItem != null)
                {
                    return _tokenItem;
                }
                var field = this.InnerItem.Fields["Token"];
                if (field == null || string.IsNullOrEmpty(field.Value))
                {
                    return null;
                }
                var id = ID.Null;
                ID.TryParse(field.Value, out id);
                if (ID.IsNullOrEmpty(id))
                {
                    return null;
                }
                var item = Sitecore.Context.Database.GetItem(id);
                if (item == null)
                {
                    return null;
                }
                _tokenItem = item;
                return _tokenItem;
            }
        }
        public string TokenName
        {
            get 
            { 
                var tokenItem = this.TokenItem;
                if (tokenItem == null)
                {
                    return null;
                }
                return tokenItem.TokenName;
            }
        }
        public string TokenValue
        {
            get
            {
                var tokenItem = this.TokenItem;
                if (tokenItem == null)
                {
                    return null;
                }
                return tokenItem.TokenValue;
            }
        }
    }
}
