using System;
using Helpfulcore.Wildcards.UrlGeneration.TokenValueExtraction;
using Sitecore.Data;
using Sitecore.Links;
using Sitecore.sitecore.admin;
using Sitecore.Sites;
using Sitecore.Web;

namespace Helpfulcore.Wildcards.sitecore.admin
{
    public class WildcardTokenExtractor : AdminPage
    {
        protected global::System.Web.UI.WebControls.TextBox tbxItemID;
        protected global::System.Web.UI.WebControls.TextBox tbxPattern;
        protected global::System.Web.UI.WebControls.Button btnSubmit;
        protected global::System.Web.UI.WebControls.TextBox tbxResult;
		protected global::System.Web.UI.WebControls.TextBox tbxCurrentUrl;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.CheckSecurity();
        }

	    protected string SiteName
	    {
		    get
		    {
			    
				var siteName = this.Request.QueryString["sc_site"];
			    if (string.IsNullOrEmpty(siteName) && Sitecore.Context.Site == null)
			    {
					throw new Exception("Site context can't be resolved, pass the 'sc_site' query string parameter");
			    }

			    return string.IsNullOrEmpty(siteName) ? Sitecore.Context.Site.Name : siteName;
		    }
	    }

        protected virtual void OnSubmit(object sender, EventArgs e)
        {
            try
            {
	            using (new SiteContextSwitcher(SiteContextFactory.GetSiteContext(this.SiteName)))
	            {
		            var item = Database.GetDatabase("master").GetItem(tbxItemID.Text);
					var itemUrl = LinkManager.GetItemUrl(item);
					var value = UrlGenerationTokenValueExtractor.Current.ExtractTokenValue(tbxPattern.Text, item);
		            
					this.tbxResult.Text = value;
		            this.tbxCurrentUrl.Text = itemUrl;
	            }
            }
            catch (Exception ex)
            {
                this.tbxResult.Text = ex.ToString();
            }
        }

        public virtual string HelpText
        {
            get { return UrlGenerationTokenValueExtractor.Current.HelpText; }
        }
    }
}
