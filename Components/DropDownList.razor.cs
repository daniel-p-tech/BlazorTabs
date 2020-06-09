using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorTabs.Components
{
    public partial class DropDownList
    {
        private ElementReference m_btnToggleRef;
        private ElementReference m_divDropdownMenuRef;
        private DotNetObjectReference<DropDownList> m_componentRef;
        private Guid m_componentGuid = Guid.NewGuid();

        private bool m_isVisible = false;

        private string SettingsDropdownCssClass => m_isVisible ? "show" : null;

        [Parameter]
        public IEnumerable<string> Items { get; set; }

        [Parameter]
        public EventCallback<string> ItemSelected { get; set; }

        [Parameter]
        public string ButtonImageClass { get; set; }

        [Parameter]
        public string ButtonText { get; set; }

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
                await JSRuntime.InvokeVoidAsync("blazorTabs.registerDropdownComponent", m_btnToggleRef, m_divDropdownMenuRef, m_componentRef, m_componentGuid);
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("blazorTabs.unregisterDropdownComponent", m_componentGuid);
            }
        }

        private void OnClick(string option)
        {
            m_isVisible = false;
            ItemSelected.InvokeAsync(option);
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
            m_componentRef?.Dispose();
        }
    }
}
