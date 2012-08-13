using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Sitecore.Marketing.Wildcards.Rules
{
    public class HasWildcardItemsCondition<T> : WhenCondition<T> where T : WildcardRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");
            Assert.IsNotNull(ruleContext.Item, "ruleContext.Item");

            return (ruleContext.WildcardCount > 0);
        }
    }
}