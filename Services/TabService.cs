using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTabs.Services
{
    public class TabService
    {
        public event Action<string, string[], bool> OnOpenTab;
        public event Action OnBack;
        public event Action<int> OnTabCountChanged;

        public void OpenTab(string page, string[] args, bool resetTabs)
        {
            OnOpenTab?.Invoke(page, args, resetTabs);
        }

        public void Back()
        {
            OnBack?.Invoke();
        }

        public void TabCountChanged(int tabsNum)
        {
            OnTabCountChanged?.Invoke(tabsNum);
        }
    }
}
