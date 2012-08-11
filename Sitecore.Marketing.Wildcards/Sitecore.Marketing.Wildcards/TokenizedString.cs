using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sitecore.Marketing.Wildcards
{
    /// <summary>
    /// This class represents a string that contains tokens. This class can be used to replace tokens in the string, and to determine token values in an existing string.
    /// </summary>
    public abstract class TokenizedString<T>
    {
        protected TokenizedString(string value, T tokens)
        {
            this.ValueBeforeReplace = value;
            this.Tokens = tokens;
        }

        /// <summary>
        /// Gets the original string.
        /// </summary>
        public string ValueBeforeReplace { get; private set; }
        
        /// <summary>
        /// Gets the value after the tokens have been replaced. It is up to the class that implements this property to determine how that replace should be performed.
        /// </summary>
        public abstract string ValueAfterReplace { get; }
        
        /// <summary>
        /// Gets the original tokens.
        /// </summary>
        public T Tokens { get; private set; }

        /// <summary>
        /// Gets a value indicating whether any tokens have been specified.
        /// </summary>
        public abstract bool HasTokens { get; }

        /// <summary>
        /// Gets the value that contains ValueAfterReplace with the tokens replaced with the specified values.
        /// </summary>
        /// <example>
        /// <code>
        /// var item = Sitecore.Context.Item;
        /// var ts = WildcardManager.Provider.GetWildcardUrl(item);
        /// var tokenValues = new NameValueCollection { { "%TOKEN1%", "group2" }, { "%TOKEN2%", "blogC" } };
        /// var newUrl = ts.ReplaceTokens(tokenValues);
        /// </code>
        /// </example>
        /// <param name="values"></param>
        /// <returns></returns>
        public virtual string ReplaceTokens(NameValueCollection values)
        {
            var newValue = this.ValueAfterReplace;
            foreach(string tokenName in values.Keys)
            {
                newValue = newValue.Replace(tokenName, values[tokenName]);
            }
            return newValue;
        }

		/// <summary>
		/// Gets a collection of tokens and values from the specified url.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>a list of toke objects</returns>
		/// <example>
		///   <code>
		/// var item = Sitecore.Context.Item;
		/// var ts = WildcardManager.Provider.GetWildcardUrl(item);
		/// var url = "/blogs/groupA/blog1.aspx";
		/// var tokens = ts.FindTokenValues(url);
		///   </code>
		///   </example>
		public abstract List<Token> FindTokenValues(string value);
    }
}
