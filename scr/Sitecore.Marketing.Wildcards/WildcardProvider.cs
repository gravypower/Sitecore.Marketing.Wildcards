using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Marketing.Wildcards.Rules;
using Sitecore.Rules;
using Sitecore.Sites;

namespace Sitecore.Marketing.Wildcards
{
    /// <summary>
    /// This class provides the ablity to interact with Sitecore wildcard items. 
    /// </summary>
    public class WildcardProvider:ProviderBase
    {
        /// <summary>
        /// Gets a WildcardTokenizedString that represents the URL for the specified item. The return value is built using the Sitecore rules engine.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual WildcardTokenizedString GetWildcardUrl(Item item, SiteContext site)
        {
            Assert.ArgumentNotNull(item, "item");
            // The item's url is: /blogs/groupa/blog1.aspx
            // 1. Find the Sitecore item that handles the URL
            // 2. Find the wildcard rule that applies to that item
            var context = RunRules(item, site);
            return context.TokenizedString;
        }

        private string routesPath = "/sitecore/system/Modules/Wildcards/Routes";

        /// <summary>
        /// Gets the location of the Sitecore item under which routes are defined.
        /// </summary>
        protected virtual string RoutesPath
        {
            get { return routesPath; }
            set { routesPath = value; }
        }

        private const string QUERY_ROUTE = "{0}/*[contains(@Items, '{1}')]";

        protected virtual WildcardRuleContext RunRules(Item item, SiteContext site)
        {
            Assert.ArgumentNotNull(item, "item");
            //
            //
            var ruleContext = new WildcardRuleContext(item, site);

            if (item.Fields["RoutesPath"] != null)
            {
                RoutesPath = item.Fields["RoutesPath"].Value;
            }

            //find the first route that matches the item
            var query = string.Format(QUERY_ROUTE, RoutesPath, item.ID.ToString());
            var queryItem = Sitecore.Context.Database.SelectSingleItem(query);
            if (queryItem == null)
            {
                return ruleContext;
            }
            //
            //run the rules
            var field = queryItem.Fields["Rules"];
            if (field == null)
            {
                return ruleContext;
            }
            var rules = RuleFactory.GetRules<WildcardRuleContext>(field);
            if (rules == null || rules.Count == 0)
            {
                return null;
            }
            rules.Run(ruleContext);
            return ruleContext;
        }

    }
}
