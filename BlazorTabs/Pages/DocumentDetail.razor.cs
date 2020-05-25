using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using BlazorTabs.Models;

namespace BlazorTabs.Pages
{
    public partial class DocumentDetail
    {
        [Parameter]
        public int DocumentId { get; set; }

        private Document Document { get; set; }

        protected override void OnInitialized()
        {
            Document = SampleData.Documents.Where(d => d.DocumentId == DocumentId).FirstOrDefault();
        }
    }
}
