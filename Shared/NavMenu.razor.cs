using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BlazorTabs.Services;

namespace BlazorTabs.Shared
{
    public partial class NavMenu
    {
        private bool m_isNavMenuVisible = true;
        private string NavMenuCssClass => m_isNavMenuVisible ? "collapse" : null;

        private void ToggleNavMenu()
        {
            m_isNavMenuVisible = !m_isNavMenuVisible;
        }

        private void Back()
        {
            TabService.Back();
        }
    }
}
