using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.UI;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.Web.UI.WebControls;

namespace Sitecore.Marketing.Wildcards
{
    public class WildcardRendering
    {
        public virtual Sublayout Sublayout { get; private set; }
        public virtual NameValueCollection Parameters { get; private set; }
        public virtual Item TargetItem
        {
            get
            {
                var targetItemId = this.Parameters["Target Item"];
                if (targetItemId == null)
                {
                    return null;
                }
                var targetItem = Sitecore.Context.Database.GetItem(targetItemId);
                return targetItem;
            }
        }
        public WildcardRendering(Control control)
        {
            Assert.ArgumentNotNull(control, "control");
            Assert.ArgumentCondition((control is Sublayout), "control", "control must be a Sublayout");
            var sublayout = (control as Sublayout);
            this.Sublayout = sublayout;
            if (sublayout != null && ! string.IsNullOrEmpty(sublayout.Parameters))
            {
                this.Parameters = Sitecore.Web.WebUtil.ParseUrlParameters(sublayout.Parameters);
            }
        }
        public static implicit operator WildcardRendering(Control control)
        {
            return new WildcardRendering(control);
        }
    }
}
