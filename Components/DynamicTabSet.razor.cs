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
        private enum AfterRenderActionType
        { 
            None,
            ScrollToLastTab,
        }

        [Parameter]
        public ObservableCollection<DynamicTab> Tabs { get; set; }

        protected DynamicTab m_activeTab;

        private ElementReference m_divPlaceholderRef;
        private ElementReference m_divTabSetRef;
        private DotNetObjectReference<DynamicTabSet> m_componentRef;
        private Guid m_componentGuid = Guid.NewGuid();

        private AfterRenderActionType m_afterRenderActionTypeId = AfterRenderActionType.None;

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
                await JSRuntime.InvokeVoidAsync("blazorTabs.registerDynamicTabSetComponent", m_divTabSetRef, m_divPlaceholderRef, m_componentRef, m_componentGuid);
            }

            await JSRuntime.InvokeVoidAsync("blazorTabs.updateScrollButtons", m_componentGuid);

            if (m_afterRenderActionTypeId == AfterRenderActionType.ScrollToLastTab)
            {
                await JSRuntime.InvokeVoidAsync("blazorTabs.scrollToLastTab", m_componentGuid);
            }
            m_afterRenderActionTypeId = AfterRenderActionType.None;
            Console.WriteLine("OnAfterRenderAsync");
        }

        private void Tabs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems?.Count > 0)
            {
                m_activeTab = (DynamicTab)e.NewItems[0];
                m_afterRenderActionTypeId = AfterRenderActionType.ScrollToLastTab;
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

        private string GetTabSetStyle()
        {
            return AppState.RoutingType == Models.RoutingType.Desktop && Tabs.Count > 0 
                ? null 
                : "visibility: collapse; height: 0px";
        }

        private string GetTabSetButtonWrapperClass(DynamicTab tab)
        {
            return m_activeTab == tab ? "active" : null;
        }

        private string GetTabClass(DynamicTab tab)
        {
            return tab == m_activeTab
                ? "tab-content-visible"
                : "tab-content-hidden";
        }

        private async Task SetActiveTab(DynamicTab tab)
        {
            m_activeTab = tab;

            int tabIndex = Tabs.IndexOf(m_activeTab);
            await JSRuntime.InvokeVoidAsync("blazorTabs.setActiveTab", m_componentGuid, tabIndex);

            TabService.ActiveTabChanged();
        }

        private void RemoveTab(DynamicTab tab)
        {
            int newTabIndex = Math.Min(Tabs.IndexOf(tab), Tabs.Count - 2);
            Tabs.Remove(tab);
            m_activeTab = newTabIndex >= 0 ? Tabs[newTabIndex] : null;

            TabService.ActiveTabChanged();
        }

        private void TabService_OnBack()
        {
            if (Tabs.Count() > 1)
            {
                RemoveTab(Tabs.Last());
            }

            TabService.TabCountChanged(Tabs.Count());
            StateHasChanged();
        }

        private async Task ScrollLeft()
        {
            await JSRuntime.InvokeVoidAsync("blazorTabs.scrollLeftDynamicTabSet", m_componentGuid);
        }

        private async Task ScrollRight()
        {
            await JSRuntime.InvokeVoidAsync("blazorTabs.scrollRightDynamicTabSet", m_componentGuid);
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
