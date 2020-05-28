using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTabs.Models;

namespace BlazorTabs.Pages
{
    public partial class Documents
    {
        public IEnumerable<Document> DocumentsData = SampleData.Documents;

        protected void OnClick(string page, params object[] args)
        {
            TabService.OpenTab(page, args);
        }
    }
}
