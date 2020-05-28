using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTabs.Services
{
    public class TabService
    {
        public event Action<string, string[]> OnOpenTab;
        public event Action OnBack;

        public void OpenTab(string page, string[] args)
        {
            OnOpenTab?.Invoke(page, args);
        }

        public void Back()
        {
            OnBack?.Invoke();
        }
    }
}
