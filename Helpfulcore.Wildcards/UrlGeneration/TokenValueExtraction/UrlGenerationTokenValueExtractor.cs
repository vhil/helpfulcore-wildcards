using Sitecore.Configuration;

namespace Helpfulcore.Wildcards.UrlGeneration.TokenValueExtraction
{
    public class UrlGenerationTokenValueExtractor
    {

        private static readonly ProviderHelper<TokenValueExtractor, TokenValueExtractorCollection> Configuration;

        static UrlGenerationTokenValueExtractor()
        {
            Configuration = new ProviderHelper<TokenValueExtractor, TokenValueExtractorCollection>("urlGenerationTokenValueExtractor");
        }

        public static TokenValueExtractor Current
        {
            get { return Configuration.Provider; }
        }

        public static TokenValueExtractorCollection Providers
        {
            get { return Configuration.Providers; }
        }
    }
}