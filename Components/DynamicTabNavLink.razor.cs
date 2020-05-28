using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorTabs.Components
{
    public partial class DynamicTabNavLink
    {
        [Parameter]
        public string Page { get; set; }

        [Parameter]
        public string Text { get; set; }

        protected override void OnInitialized()
        {
            AppState.OnRoutingTypeChanged += AppState_OnRoutingTypeChanged;
        }

        private void AppState_OnRoutingTypeChanged()
        {
            StateHasChanged();
        }

        private void OnClick()
        {
            TabService.OpenTab(Page, null);
        }
    }
}
