using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;

namespace Sitecore.Marketing.Wildcards.Controls
{
    using System;

    public partial class DisplayDynamicUrls : System.Web.UI.UserControl
    {
        private void Page_Load(object sender, EventArgs e)
        {
            var sampleData = GetSampleData();
            WildcardRendering rendering = this.Parent;
            if (rendering != null)
            {
                var targetItem = rendering.TargetItem;
                if (targetItem != null)
                {
                    const string description = "This example uses the target item specified on the rendering.";
                    Literal1.Text = GetItemInformation(targetItem, description, sampleData);
                }
            }
        }
        
        private string GetItemInformation(Item item, string description, ICollection<NameValueCollection> sampleData)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNull(description, "description");
            //
            //
            var ts = WildcardManager.Provider.GetWildcardUrl(item, Sitecore.Context.Site);
            //
            //
            var builder = new StringBuilder();
            builder.Append("<table border='1'>");
            builder.AppendFormat("<tr><th colspan='2'>{0}</th></tr>", description);
            var url = LinkManager.GetItemUrl(item);

           
            builder.AppendFormat("<tr><td>Target item path</td><td><code>{0}</code></td></tr>", item.Paths.FullPath);
            builder.AppendFormat("<tr><td>URL before rules</td><td><code>{0}</code></td></tr>", ts.ValueBeforeReplace);
            if (!ts.HasTokens)
            {
                builder.Append("<tr><td>Note</td><td style='color:red'><code>The specified item has no tokens.</code></td></tr>");
            }
            builder.AppendFormat("<tr><td>URL after rules</td><td><code>{0}</code></td></tr>", ts.ValueAfterReplace);
            //
            //
            if (sampleData != null && sampleData.Count > 0)
            {
                var builder2 = new StringBuilder();
                foreach (var row in sampleData)
                {
                    builder2.AppendFormat("<a href=\"{0}\">{0}</a><br/>", ts.ReplaceTokens(row));
                }
                builder.AppendFormat("<tr><td>Samples</td><td><code>{0}</code></td></tr>", builder2.ToString());
            }
            //
            //
            builder.Append("</table>");
            return builder.ToString();
        }

        //The following constants must match tokens defined in Sitecore
        const string TOKEN_GROUP_NAME = "%Group Name%";
        const string TOKEN_BLOG_NAME = "%Blog Name%";
        
        private List<NameValueCollection> GetSampleData()
        {
            var list = new List<NameValueCollection>();
            list.Add(new NameValueCollection { { TOKEN_GROUP_NAME, "group1" }, { TOKEN_BLOG_NAME, "blogA" } });
            list.Add(new NameValueCollection { { TOKEN_GROUP_NAME, "group1" }, { TOKEN_BLOG_NAME, "blogB" } });
            list.Add(new NameValueCollection { { TOKEN_GROUP_NAME, "group2" }, { TOKEN_BLOG_NAME, "blogC" } });
            return list;
        }
    }
}