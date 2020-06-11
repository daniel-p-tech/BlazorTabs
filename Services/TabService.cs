using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTabs.Components;

namespace BlazorTabs.Services
{
    public class TabService
    {
        public event Action<string, string[], bool> OnOpenTab;
        public event Action OnActiveTabChanged;
        public event Action OnTabSetResized;
        public event Action<int> OnTabCountChanged;
        public event Action OnBack;

        public void OpenTab(string page, string[] args, bool resetTabs)
        {
            OnOpenTab?.Invoke(page, args, resetTabs);
        }

        public void ActiveTabChanged()
        {
            OnActiveTabChanged?.Invoke();
        }

        public void TabSetResized()
        {
            OnTabSetResized?.Invoke();
        }

        public void TabCountChanged(int tabsNum)
        {
            OnTabCountChanged?.Invoke(tabsNum);
        }

        public void Back()
        {
            OnBack?.Invoke();
        }
    }
}
