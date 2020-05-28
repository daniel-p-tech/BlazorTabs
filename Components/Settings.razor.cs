using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTabs.Models;

namespace BlazorTabs.Components
{
    public partial class Settings
    {
        private bool m_isSettingsVisible = false;

        private string SettingsDropdownCssClass => m_isSettingsVisible ? "show" : null;

        private void ToggleSettings()
        {
            m_isSettingsVisible = !m_isSettingsVisible;
        }

        private void OnClick(string option)
        {
            AppState.SetRoutingType((RoutingType)(Enum.Parse(typeof(RoutingType), option)));
            m_isSettingsVisible = false;
        }
    }
}
