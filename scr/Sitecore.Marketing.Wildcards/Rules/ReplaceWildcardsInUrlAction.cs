using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Actions;

namespace Sitecore.Marketing.Wildcards.Rules
{
    public class ReplaceWildcardsInUrlAction<T> : RuleAction<T> where T : WildcardRuleContext
    {
        public string Mappings { get; set; }

        public override void Apply([NotNull] T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");
            Assert.IsNotNull(ruleContext.Item, "ruleContext.Item");
            Assert.IsNotNull(ruleContext.Tokens, "ruleContext.Tokens");
            //
            //get the token value
            var mappingsItem = Sitecore.Context.Database.GetItem(new ID(this.Mappings));
            Assert.IsNotNull(mappingsItem, "The mappings item {0} cannot be found in the database.", this.Mappings);
            if (mappingsItem.HasChildren)
            {
                foreach(Item child in mappingsItem.Children)
                {
                    MappingItem mapping = child;
                    if (mapping == null)
                    {
                        continue;
                    }
					//add a blank data source and switch context to keep origan function intact
                    ruleContext.Tokens.Add(mapping.Position, new Token(mapping.TokenValue, "", "", ""));
                }
            }
        }
    }
}