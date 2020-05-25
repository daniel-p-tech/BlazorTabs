using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTabs.Services
{
    public class TabService
    {
        public event Action<string, object[]> OnOpenTab;

        public void OpenTab(string page, object[] args)
        {
            OnOpenTab?.Invoke(page, args);
        }
    }
}
