using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BlazorTabs.Models;
using BlazorTabs.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorTabs.Shared
{
    public partial class NavMenu
    {
        private bool m_isNavMenuVisible = true;
        private string NavMenuCssClass => m_isNavMenuVisible ? "collapse" : null;

        private bool IsBackButtonMenuVisible { get; set; } = true;

        private string[] RoutingTypes = new string[] { "Default", "Desktop", "Mobile" };

        protected override void OnInitialized()
        {
            IsBackButtonMenuVisible = AppState.RoutingType == RoutingType.Mobile;
            AppState.OnRoutingTypeChanged += AppState_OnRoutingTypeChanged;
            TabService.OnTabCountChanged += TabService_OnTabCountChanged;
        }

        private void ToggleNavMenu()
        {
            m_isNavMenuVisible = !m_isNavMenuVisible;
        }

        private void Back()
        {
            TabService.Back();
        }

        private void AppState_OnRoutingTypeChanged()
        {
            IsBackButtonMenuVisible = false;
        }

        private void TabService_OnTabCountChanged(int tabsNum)
        {
            IsBackButtonMenuVisible = AppState.RoutingType == RoutingType.Mobile && tabsNum > 1;
            StateHasChanged();
        }

        private void RoutingTypeChanged(string option)
        {
            AppState.SetRoutingType((RoutingType)(Enum.Parse(typeof(RoutingType), option)));
            NavigationManager.NavigateTo("/");
            StateHasChanged();
        }
    }
}
