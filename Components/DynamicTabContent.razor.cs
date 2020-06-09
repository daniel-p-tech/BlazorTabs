using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorTabs.Components
{
    public partial class DynamicTabContent
    {
        private ElementReference m_divContentRef;
        private DotNetObjectReference<DynamicTabContent> m_componentRef;
        private Guid m_componentGuid = Guid.NewGuid();

        private string Height { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string CssClass { get; set; } = "tabcontent-visible";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                m_componentRef = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("blazorTabs.registerDynamicTabContentComponent", m_divContentRef, m_componentRef, m_componentGuid);
            }
        }

        [JSInvokable]
        public void Resize(int height)
        {
            Height = height + "px";
            StateHasChanged();
        }

        public void Dispose()
        {
            m_componentRef?.Dispose();
        }
    }
}
