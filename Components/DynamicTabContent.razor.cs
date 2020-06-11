using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorTabs.Components
{
    public partial class DynamicTabContent
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter]
        public DynamicTab ParentTab { get; set; }

        [CascadingParameter]
        public DynamicTabSet DynamicTabSet { get; set; }

        private string Height { get; set; }

        protected override void OnInitialized()
        {
            TabService.OnActiveTabChanged += TabService_OnActiveTabChanged;
            TabService.OnTabSetResized += TabService_OnTabSetResized;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Height = await JSRuntime.InvokeAsync<int>("blazorTabs.getDynamicTabSetComponentHeight", DynamicTabSet.ComponentGuid) + "px";
                StateHasChanged();
            }
        }

        private void TabService_OnActiveTabChanged()
        {
            if (DynamicTabSet.ActiveTab == ParentTab)
            {
                Height = DynamicTabSet.ContentHeight + "px";
                StateHasChanged();
            }
        }

        private void TabService_OnTabSetResized()
        {
            if (DynamicTabSet.ActiveTab == ParentTab)
            {
                Height = DynamicTabSet.ContentHeight + "px";
                StateHasChanged();
            }
        }
    }
}
