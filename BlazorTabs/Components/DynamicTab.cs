using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorTabs.Components
{
    public class DynamicTab : ComponentBase
    {
        public string Title { get; set; }
        public ComponentBase Component { get; }
        public Dictionary<string, object> Parameters { get; set; }

        public DynamicTab(ComponentBase component, string title, Dictionary<string, object> parameters = null)
        {
            Component = component;
            Title = title;
            Parameters = parameters;
        }
    }
}
