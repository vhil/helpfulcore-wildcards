using System.Configuration.Provider;
using Sitecore.Data.Items;

namespace Helpfulcore.Wildcards.UrlGeneration.TokenValueExtraction
{
    public class TokenValueExtractor : ProviderBase
    {
        public virtual string ExtractTokenValue(string tokenPattern, Item item)
        {
            return string.Empty;
        }

        public virtual string HelpText
        {
            get { return "This token extractor does not have explanation text."; }
        }
    }
}
