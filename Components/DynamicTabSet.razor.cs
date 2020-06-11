using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorTabs.Components
{
    public partial class DynamicTabSet
    {
        [Parameter]
        public ObservableCollection<DynamicTab> Tabs { get; set; }

        protected DynamicTab m_activeTab;

        private ElementReference m_divPlaceholderRef;
        private DotNetObjectReference<DynamicTabSet> m_componentRef;
        private Guid m_componentGuid = Guid.NewGuid();

        public int ContentHeight { get; set; } = 0;
        public DynamicTab ActiveTab { get { return m_activeTab; } }
        public Guid ComponentGuid {  get { return m_componentGuid; } }

        protected override void OnInitialized()
        {
            if (Tabs.Count > 0)
            {
                m_activeTab = Tabs.Last();
                TabService.ActiveTabChanged();
            }
            Tabs.CollectionChanged += Tabs_CollectionChanged;
            TabService.OnBack += TabService_OnBack;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                m_componentRef = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("blazorTabs.registerDynamicTabSetComponent", m_divPlaceholderRef, m_componentRef, m_componentGuid);
            }
        }

        private void Tabs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems?.Count > 0)
            {
                m_activeTab = (DynamicTab)e.NewItems[0];
                TabService.ActiveTabChanged();
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
            TabService.ActiveTabChanged();
            StateHasChanged();
        }

        protected void RemoveTab(DynamicTab tab)
        {
            int newTabIndex = Math.Min(Tabs.IndexOf(tab), Tabs.Count - 2);
            Tabs.Remove(tab);
            m_activeTab = newTabIndex >= 0 ? Tabs[newTabIndex] : null;
            TabService.ActiveTabChanged();
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

        [JSInvokable]
        public void SetContentHeight(int height)
        {
            ContentHeight = height;
            TabService.TabSetResized();
        }

        public void Dispose()
        {
            m_componentRef?.Dispose();
        }
    }
}
