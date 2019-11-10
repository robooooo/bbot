using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;

namespace BBotCore
{
    // Used in $search and $SCP commands to search the web
    public class SearchHelper
    {
        private CustomsearchService CSS;

        public SearchHelper(CustomsearchService css) {
            CSS = css;
        }

        public async Task<List<String>> AsyncSearchFor(string query) {
            // TODO: Implement web scraping approach
            return await AsyncCustomSearch(query);
        }

        // Legacy approach using google's custom search service to search w/ no website specific
        private async Task<List<String>> AsyncCustomSearch(string query, int results = 10) {
            var CSE = CSS.Cse.List(query);
            CSE.Cx = GetSearchCX();
            var Results = await CSE.ExecuteAsync();
            return Results.Items.Take(results).Select(r => r.FormattedUrl).ToList();
        }

        private string GetSearchCX() => Environment.GetEnvironmentVariable("SEARCH_CX");
    }
}