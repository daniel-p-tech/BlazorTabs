using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BlazorTabs.Components;

namespace BlazorTabs.Pages
{
    public partial class Index
    {
        protected ObservableCollection<DynamicTab> m_tabs = new ObservableCollection<DynamicTab>();

        protected override void OnInitialized()
        {
            AppState.OnRoutingTypeChanged += AppState_OnRoutingTypeChanged;
            TabService.OnOpenTab += TabService_OnOpenTab;
        }

        private void AppState_OnRoutingTypeChanged()
        {
            m_tabs.Clear();

            if (AppState.RoutingType == Models.RoutingType.Desktop)
            {
                for (int i = 1; i <= 10; i++)
                {
                    m_tabs.Add(new DynamicTab(new Documents(), "Documents - " + i.ToString("##")));
                }
            }

            StateHasChanged();
        }

        private void TabService_OnOpenTab(string page, string[] args, bool resetTabs)
        {
            var parameters = new Dictionary<string, object>();

            if (resetTabs == true)
            {
                m_tabs.Clear();
            }

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
                    int documentId = int.Parse(args[0]);
                    parameters.Add("DocumentId", documentId);
                    m_tabs.Add(new DynamicTab(new DocumentDetail(), "Document - " + documentId, parameters));
                    break;
            }

            TabService.TabCountChanged(m_tabs.Count());
            StateHasChanged();
        }
    }
}
