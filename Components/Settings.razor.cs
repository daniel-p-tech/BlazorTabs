using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using BlazorTabs.Models;

namespace BlazorTabs.Components
{
    public partial class Settings
    {
        private ElementReference m_btnToggleRef;
        private ElementReference m_divDropdownMenuRef;
        private DotNetObjectReference<Settings> m_componentRef;

        private bool m_isVisible = false;

        private string SettingsDropdownCssClass => m_isVisible ? "show" : null;

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        m_componentRef = DotNetObjectReference.Create(this);
        //        await JSRuntime.InvokeVoidAsync("blazorTabs.registerClosableComponent", m_divSettingsRef, m_componentRef);
        //    }
        //}

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                m_componentRef = DotNetObjectReference.Create(this);
            }
        }

        private async Task ToggleVisible()
        {
            m_isVisible = !m_isVisible;
            await SetVisible();
        }

        private async Task SetVisible()
        {
            if (m_isVisible)
            {
                await JSRuntime.InvokeVoidAsync("blazorTabs.registerDropdownComponent", m_btnToggleRef, m_divDropdownMenuRef, m_componentRef);
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("blazorTabs.unregisterDropdownComponent", m_componentRef);
            }
        }

        private void OnClick(string option)
        {
            AppState.SetRoutingType((RoutingType)(Enum.Parse(typeof(RoutingType), option)));
            m_isVisible = false;
        }

        [JSInvokable]
        public async Task CloseDropdown()
        {
            m_isVisible = false;
            await SetVisible();
            StateHasChanged();
        }

        public void Dispose()
        {
            //Task.Run(async () => await JSRuntime.InvokeVoidAsync("blazorTabs.unregisterDropdownComponent", m_componentRef));
            m_componentRef?.Dispose();
        }

        //public async Task DisposeAsync()
        //{
        //    await JSRuntime.InvokeVoidAsync("blazorTabs.unregisterDropdownComponent", m_componentRef);
        //    m_componentRef?.Dispose();
        //}
    }
}
