using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sitecore.Marketing.Wildcards
{

	/// <summary>
	/// This class give us the ability to pass more information around when working with tokens
	/// </summary>
	public class Token
	{
		public string TokenString { get; set; }
		public string TokenValue { get; set; }
		public string DataSourceString { get; set; }
        public string SwitchContextItemString { get; set; }
        public string CustomFindMethodString { get; set; }

        public Token(string tokenString, string dataSourceString, string switchContextItemString, string customFindMethod)
		{
			TokenString = tokenString;
			DataSourceString = dataSourceString;
            SwitchContextItemString = switchContextItemString;
            CustomFindMethodString = customFindMethod;
		}
	}
}
