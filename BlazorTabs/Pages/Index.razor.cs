using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BlazorTabs.Components;

namespace BlazorTabs.Pages
{
    public partial class Index
    {
        protected ObservableCollection<DynamicTab> m_tabs = new ObservableCollection<DynamicTab>();

        protected override void OnInitialized()
        {
            TabService.OnOpenTab += TabService_OnOpenTab;
        }

        private void TabService_OnOpenTab(string page, object[] args)
        {
            switch (page)
            {
                case "Counter":
                    m_tabs.Add(new DynamicTab(new Counter(), page));
                    break;
                case "FetchData":
                    m_tabs.Add(new DynamicTab(new FetchData(), page));
                    break;
                case "Documents":
                    m_tabs.Add(new DynamicTab(new Documents(), page));
                    break;
                case "DocumentDetail":
                    int documentId = (int)args[0];
                    var parameters = new Dictionary<string, object>();
                    parameters.Add("DocumentId", documentId);
                    m_tabs.Add(new DynamicTab(new DocumentDetail(), "Document - " + documentId, parameters));
                    break;
            }

            StateHasChanged();
        }
    }
}
