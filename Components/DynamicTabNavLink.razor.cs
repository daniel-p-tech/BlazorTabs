using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using BlazorTabs.Services;

namespace BlazorTabs.Components
{
    public partial class DynamicTabNavLink
    {
        [Parameter]
        public string Page { get; set; }

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public bool IsNavBarLink { get; set; } = false;

        public string CssClass { get { return IsNavBarLink ? "nav-link text-light" : null; } }

        protected override void OnInitialized()
        {
            AppState.OnRoutingTypeChanged += AppState_OnRoutingTypeChanged;
        }

        private void AppState_OnRoutingTypeChanged()
        {
            StateHasChanged();
        }

        private void OnClick(bool resetTabs)
        {
            string[] pageInfo = Page.Split("/");
            TabService.OpenTab(pageInfo[0], pageInfo.Count() == 1 ? null : pageInfo.Skip(1).ToArray(), resetTabs);
        }
    }
}
