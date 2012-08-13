using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Configuration;

namespace Sitecore.Marketing.Wildcards
{
    public static class WildcardManager
    {
        private static readonly ProviderHelper<WildcardProvider, WildcardProviderCollection> _helper;
        static WildcardManager()
        {
            _helper = new ProviderHelper<WildcardProvider, WildcardProviderCollection>("wildcardManager");
        }
        public static WildcardProvider Provider
        {
            get
            {
                return _helper.Provider;
            }
        }
        public static WildcardProviderCollection Providers
        {
            get
            {
                return _helper.Providers;
            }
        }
    }
}
