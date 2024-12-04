using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Archiventure
{
    public class TabGroup : MonoBehaviour
    {
        [HideInInspector] public List<TapButton> TapButtons;
        public Sprite tabIdle;
        public Sprite tabHover;
        public Sprite tabActive;
        public TapButton selectedTab;
        public List<GameObject> objectsToSwap;

        void Start()
        {
            ResetTabs();
        }

        public void Subscribe(TapButton button)
        {
            if (TapButtons == null)
            {
                TapButtons = new List<TapButton>();
            }
            TapButtons.Add(button);
        }

        public void OnTabEnter(TapButton button)
        {
            ResetTabs();
            if (selectedTab == null || button != selectedTab)
            {
                button.background.sprite = tabHover;
            }
        }

        public void OnTabExit(TapButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(TapButton button)
        {
            selectedTab = button;
            ResetTabs();
            button.background.sprite = tabActive;
            int index = button.transform.GetSiblingIndex();
            for (int i = 0; i < objectsToSwap.Count; i++)
            {
                if (i == index)
                {
                    objectsToSwap[i].SetActive(true);
                }
                else
                {
                    objectsToSwap[i].SetActive(false);
                }
            }
        }

        public void ResetTabs()
        {
            foreach (TapButton button in TapButtons)
            {
                if (selectedTab != null && button == selectedTab) { continue; }
                button.background.sprite = tabIdle;
            }
        }
    }
}