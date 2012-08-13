using System;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Sitecore.Marketing.Wildcards.Controls
{
    public partial class DisplayTokenValues : System.Web.UI.UserControl
    {
        private void Page_Load(object sender, EventArgs e)
        {
            const string description = "This example uses the current URL.";
            Literal1.Text = GetUrlInformation(Sitecore.Context.RawUrl, Sitecore.Context.Item, description);
        }

        private string GetUrlInformation(string url, Item item, string description)
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
            builder.AppendFormat("<tr><td>Current URL</td><td><code>{0}</code></td></tr>", url);
            builder.AppendFormat("<tr><td>Sitecore item path</td><td><code>{0}</code></td></tr>", item.Paths.FullPath);
            builder.AppendFormat("<tr><td>URL before rules</td><td><code>{0}</code></td></tr>", ts.ValueBeforeReplace);
            builder.AppendFormat("<tr><td>URL after rules</td><td><code>{0}</code></td></tr>", ts.ValueAfterReplace);
            //
            //
            if (ts.HasTokens)
            {
                var builder2 = new StringBuilder();
                var tokens = ts.FindTokenValues(url);
                builder2.Append("<table border='0'>");
                foreach (var t in tokens)
                {
                    builder2.AppendFormat("<tr><td>{0}</td><td>&rarr; {1}</td></tr>", t.TokenValue, t.TokenString);
                }
                builder2.Append("</table>");
                builder.AppendFormat("<tr><td>Tokens:</td><td><code>{0}</code></td></tr>", builder2.ToString());
            }
            else
            {
                builder.Append("<tr><td>Note:</td><td style='color:red'><code>The specified item has no tokens.</code></td></tr>");
            }
            builder.Append("</table>");
            return builder.ToString();
        }
    }
}