using System;
using Helpfulcore.Wildcards.UrlGeneration.TokenValueExtraction;
using Sitecore.Data;
using Sitecore.sitecore.admin;

namespace Helpfulcore.Wildcards.Profiling
{
    public class WildcardTokenExtractor : AdminPage
    {
        protected global::System.Web.UI.WebControls.TextBox tbxItemID;
        protected global::System.Web.UI.WebControls.TextBox tbxPattern;
        protected global::System.Web.UI.WebControls.Button btnSubmit;
        protected global::System.Web.UI.WebControls.TextBox tbxResult;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.CheckSecurity();
        }

        protected virtual void OnSubmit(object sender, EventArgs e)
        {
            try
            {
                var item = Database.GetDatabase("master").GetItem(tbxItemID.Text);
                var value = UrlGenerationTokenValueExtractor.Current.ExtractTokenValue(tbxPattern.Text, item);
                this.tbxResult.Text = value;
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
