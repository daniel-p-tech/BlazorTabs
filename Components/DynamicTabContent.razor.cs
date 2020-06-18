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

        private int Height { get; set; }

        protected override void OnInitialized()
        {
            TabService.OnActiveTabChanged += UpdateHeight;
            TabService.OnTabSetResized += UpdateHeight;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Height = DynamicTabSet.ContentHeight = await JSRuntime.InvokeAsync<int>("blazorTabs.getDynamicTabSetComponentHeight", DynamicTabSet.ComponentGuid);
                StateHasChanged();
            }
        }

        private void UpdateHeight()
        {
            if (DynamicTabSet.ActiveTab == ParentTab)
            {
                Height = DynamicTabSet.ContentHeight;
                StateHasChanged();
            }
        }
    }
}
