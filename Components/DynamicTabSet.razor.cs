using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace BlazorTabs.Components
{
    public partial class DynamicTabSet
    {
        [Parameter]
        public ObservableCollection<DynamicTab> Tabs { get; set; }

        protected DynamicTab m_activeTab;

        protected override void OnInitialized()
        {
            if (Tabs.Count > 0)
            {
                m_activeTab = Tabs.Last();
            }
            Tabs.CollectionChanged += Tabs_CollectionChanged;
            TabService.OnBack += TabService_OnBack;
        }

        private void Tabs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems?.Count > 0)
            {
                m_activeTab = (DynamicTab)e.NewItems[0];
            }
        }

        protected RenderFragment RenderTabComponent(DynamicTab tab) => builder =>
        {
            int sequence = 0;
            builder.OpenComponent(sequence++, tab.Component.GetType());

            if (tab.Parameters != null)
            {
                foreach (var parameter in tab.Parameters)
                {
                    builder.AddAttribute(sequence++, parameter.Key, parameter.Value);
                }
            }

            builder.CloseComponent();
        };

        protected string GetTabButtonStyle(DynamicTab tab)
        {
            return m_activeTab == tab
                ? "background-color: #ccc"
                : "background-color: inherit";
        }

        protected string GetTabClass(DynamicTab tab)
        {
            return tab == m_activeTab
                ? "tabcontent-visible"
                : "tabcontent-hidden";
        }

        protected void SetActiveTab(DynamicTab tab)
        {
            m_activeTab = tab;
            StateHasChanged();
        }

        protected void RemoveTab(DynamicTab tab)
        {
            int newTabIndex = Math.Min(Tabs.IndexOf(tab), Tabs.Count - 2);
            Tabs.Remove(tab);
            m_activeTab = newTabIndex >= 0 ? Tabs[newTabIndex] : null;
            StateHasChanged();
        }

        private void TabService_OnBack()
        {
            if (Tabs.Count() > 1)
            {
                RemoveTab(Tabs.Last());
            }

            TabService.TabCountChanged(Tabs.Count());
        }
    }
}
