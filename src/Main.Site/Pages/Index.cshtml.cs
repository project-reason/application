using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Main.Site.Pages
{
    public class IndexModel : PageModel
    {
        private ILogger<IndexModel> _Logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _Logger = logger;
        }


        public void OnGet()
        {
            _Logger.LogDebug("Index has been requested");
        }


        public int Add(int x, int y)
        {
            return x + y;
        }
    }
}
