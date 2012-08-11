using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;

namespace Sitecore.Marketing.Wildcards.Configuration
{
    public static class Settings
    {
        public static class Tokenize
        {
            public static string WildcardTokenizedString { get { return Sitecore.Configuration.Settings.GetSetting("WildcardTokenizedString", ",-w-,"); } }
			public static readonly Sitecore.Data.TemplateID Wildcard = new Sitecore.Data.TemplateID(new ID("{0808ACC4-CB45-4B03-9EB5-737E565DD444}"));
        }
    }
}
