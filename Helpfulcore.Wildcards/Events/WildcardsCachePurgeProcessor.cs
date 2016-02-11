using System;

namespace Helpfulcore.Wildcards.Events
{
    public class WildcardsCachePurgeProcessor
    {
        public void ClearCache(object sender, EventArgs args)
        {
            WildcardManager.ClearCache();
        }
    }
}
