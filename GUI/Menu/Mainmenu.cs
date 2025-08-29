namespace MMAR.GUI.Menu
{
    using EditorAttributes;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    public class Mainmenu : MonoBehaviour
    {
        public List<MenuItem> menuItems;
        public MenuItemUI menuItemUIPrefab;
        public Transform contentParent;
        [Header("UI Properties")]
        public float itemSpacing = 10f;
        public float itemHeight = 30f;
        public float sideMargin = 10f;
        [Button]
        public void GenerateMenu()
        {
            // Collect existing MenuItemUI children and non-MenuItemUI children
            List<MenuItemUI> existingUIs = new List<MenuItemUI>();
            List<Transform> toRemove = new List<Transform>();

            foreach (Transform child in contentParent)
            {
                var ui = child.GetComponent<MenuItemUI>();
                if (ui != null)
                    existingUIs.Add(ui);
                else
                    toRemove.Add(child);
            }

            // Destroy non-MenuItemUI children
            foreach (var t in toRemove)
                DestroyImmediate(t.gameObject);

            float y = -sideMargin;
            int i = 0;
            // Reuse or create MenuItemUI for each menu item
            for (; i < menuItems.Count; i++)
            {
                MenuItem item = menuItems[i];
                MenuItemUI ui;
                if (i < existingUIs.Count)
                {
                    ui = existingUIs[i];
                    ui.gameObject.SetActive(true);
                }
                else
                {
                    ui = Instantiate(menuItemUIPrefab, contentParent);
                }
                ui.SetData(item);
                ui.gameObject.name = item.name + " Button"; // Update name for clarity
                // Positioning
                var rect = ui.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(1, 1);
                rect.pivot = new Vector2(0.5f, 1);
                rect.anchoredPosition = new Vector2(0, y);
                rect.sizeDelta = new Vector2(-2 * sideMargin, itemHeight);

                y -= (itemHeight + itemSpacing);
            }

            // Destroy extra MenuItemUI if any
            for (; i < existingUIs.Count; i++)
            {
                DestroyImmediate(existingUIs[i].gameObject);
            }
        }
    }

    [System.Serializable]
    public class MenuItem
    {
        public string name;
        public UnityEvent actions;
    }
}