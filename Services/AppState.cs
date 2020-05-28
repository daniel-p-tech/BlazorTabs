using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTabs.Models;

namespace BlazorTabs.Services
{
    public class AppState
    {
        public RoutingType RoutingType { get; set; } = RoutingType.Default;

        public event Action OnRoutingTypeChanged;

        public void SetRoutingType(RoutingType routingType)
        {
            RoutingType = routingType;
            OnRoutingTypeChanged?.Invoke();
        }
    }
}
