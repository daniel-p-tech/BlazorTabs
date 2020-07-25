using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorTabs.Models;

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

        private ElementReference m_divTabSetRef;
        private ElementReference m_divPlaceholderRef;
        private ElementReference m_btnLeftScroll;
        private ElementReference m_btnRightScroll;
        private DotNetObjectReference<DynamicTabSet> m_componentRef;
        private Guid m_componentGuid = Guid.NewGuid();
        private int m_scrollLoopId = -1;

        private bool m_suppressRender = false;
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

        protected override bool ShouldRender()
        {
            Console.WriteLine("ShouldRender => " + !m_suppressRender);
            bool suppressRender = m_suppressRender;
            m_suppressRender = false;
            return !suppressRender;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                m_componentRef = DotNetObjectReference.Create(this);
                JSRuntime.InvokeVoid("blazorTabs.registerDynamicTabSetComponent", m_divTabSetRef, m_divPlaceholderRef, m_btnLeftScroll, m_btnRightScroll, m_componentRef, m_componentGuid);
            }

            JSRuntime.InvokeVoid("blazorTabs.updateScrollButtons", m_componentGuid);

            if (m_afterRenderActionTypeId == AfterRenderActionType.ScrollToLastTab)
            {
                JSRuntime.InvokeVoid("blazorTabs.scrollToLastTab", m_componentGuid);
            }

            m_afterRenderActionTypeId = AfterRenderActionType.None;
            m_suppressRender = false;
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

        private string GetTabSetWrapperStyle()
        {
            return AppState.RoutingType == RoutingType.Desktop && Tabs.Count > 0 
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
                ? (AppState.RoutingType == RoutingType.Desktop ? "tab-content-visible-desktop" : "tab-content-visible-mobile")
                : "tab-content-hidden";
        }

        private void SetActiveTab(DynamicTab tab)
        {
            if (m_activeTab != tab)
            {
                m_activeTab = tab;

                JSRuntime.InvokeVoid("blazorTabs.setActiveTab", m_componentGuid, Tabs.IndexOf(m_activeTab));

                TabService.ActiveTabChanged();
            }
            else
            {
                m_suppressRender = true;
            }
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

        private void ScrollLeft()
        {
            Console.WriteLine("ScrollLeftAsync");
            m_scrollLoopId = JSRuntime.Invoke<int>("blazorTabs.scrollLeftDynamicTabSet", m_componentGuid);
            m_suppressRender = true;
        }

        private void ScrollRight()
        {
            Console.WriteLine("ScrollRightAsync");
            m_scrollLoopId = JSRuntime.Invoke<int>("blazorTabs.scrollRightDynamicTabSet", m_componentGuid);
            m_suppressRender = true;
        }

        private void ScrollStop()
        {
            Console.WriteLine("ScrollStopAsync");
            JSRuntime.InvokeVoid("blazorTabs.stopDynamicTabSetScrolling", m_scrollLoopId);
            m_suppressRender = true;
        }

        [JSInvokable]
        public void SetContentHeight(int height)
        {
            ContentHeight = height;

            TabService.TabSetResized();
        }

        [JSInvokable]
        public int GetScrollLoopId()
        {
            return m_scrollLoopId;
        }

        public void Dispose()
        {
            JSRuntime.InvokeVoid("blazorTabs.unregisterDynamicTabSetComponent", m_componentGuid);
            m_componentRef?.Dispose();
        }
    }
}
