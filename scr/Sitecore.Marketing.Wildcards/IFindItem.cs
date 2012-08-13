using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Sitecore.Marketing.Wildcards
{
    public interface IFindItem
    {
        Item[] FindItems(string tokenValue);
    }
}
