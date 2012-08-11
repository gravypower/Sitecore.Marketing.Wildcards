using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sitecore.Marketing.Wildcards
{
	public class WildcardTokenizedString : TokenizedString<Dictionary<int, Token>>
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="WildcardTokenizedString"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="tokens">The tokens.</param>
		public WildcardTokenizedString(string value, Dictionary<int, Token> tokens)
			: base(value, tokens)
        {
        }

		/// <summary>
		/// Tokenizes this instance.
		/// </summary>
        protected virtual void Tokenize()
        {
            var transformed = new StringBuilder();
            var pieces = Regex.Split(this.ValueBeforeReplace, Sitecore.Marketing.Wildcards.Configuration.Settings.Tokenize.WildcardTokenizedString);
            for (var i = 0; i < pieces.Length; i++)
            {
                if (this.Tokens.ContainsKey(i))
                {
                    transformed.Append(this.Tokens[i].TokenString);
                }
                transformed.Append(pieces[i]);
            }
            _valueAfterReplace = transformed.ToString();
        }

        private string _valueAfterReplace;
        public override string ValueAfterReplace
        {
            get
            {
                if (string.IsNullOrEmpty(_valueAfterReplace))
                {
                    Tokenize();
                }
                return _valueAfterReplace; 
            }
        }
        
        public override bool HasTokens
        {
            get { return this.Tokens.Keys.Count > 0; }
        }

		/// <summary>
		/// Finds the token values.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
        public override List<Token> FindTokenValues(string value)
        {
			var collection = new List<Token>();
            var regexValue = this.ValueAfterReplace;
            foreach (var key in this.Tokens.Keys.OrderBy(p => p))
            {
                var token = this.Tokens[key];
				regexValue = regexValue.Replace(token.TokenString, @"([\S\s]*)");
            }
			//Changed to match on any case
            var regex = new Regex(regexValue, RegexOptions.IgnoreCase);
            var match = regex.Match(value);
            if (match.Success)
            {
                for (var i = 0; i < match.Groups.Count; i++)
                {
                    if (match.Groups[i].Value == value)
                    {
                        continue;
                    }
                    if (this.Tokens.ContainsKey(i))
                    {
                        var token = this.Tokens[i];
                        var groupValue = match.Groups[i].Value;

                        //remove any query string
                        if (groupValue.IndexOf('?') > 0)
                        {
                            groupValue = groupValue.Split('?')[0];
                        }

                        token.TokenValue = groupValue;

						collection.Add(token);
                    }
                }
            }
            return collection;
        }

    }
}
