using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorTabs.Components
{
    public partial class Settings
    {
        [Parameter]
        public bool Visible { get; set; } = true;

        [Parameter]
        public Action<bool> VisibleChanged { get; set; }
    }
}
