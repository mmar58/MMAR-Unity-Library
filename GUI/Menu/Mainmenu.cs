namespace MMAR.GUI.Menu
{
    using EditorAttributes;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    public enum MenuOrientation
    {
        Vertical,
        Horizontal,
        Responsive
    }

    public class Mainmenu : MonoBehaviour
    {
        public List<MenuItem> menuItems;
        public MenuItemUI menuItemUIPrefab;
        public Transform contentParent;
        [Header("Menu Orientation")]
        public MenuOrientation orientation = MenuOrientation.Vertical;
        [Header("Responsive Settings")]
        public bool isResponsive = false;
        [Header("UI Properties")]
        public float itemSpacing = 10f;
        public float itemHeight = 30f;
        public float itemWidth = 100f; // Added for horizontal layout
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

            if (menuItems.Count == 0) return;

            // Get container dimensions for responsive calculation
            RectTransform containerRect = contentParent.GetComponent<RectTransform>();
            float containerWidth = containerRect.rect.width;
            float containerHeight = containerRect.rect.height;

            // Calculate responsive dimensions if needed
            float responsiveItemWidth = itemWidth;
            float responsiveItemHeight = itemHeight;
            
            if (isResponsive || orientation == MenuOrientation.Responsive)
            {
                if (orientation == MenuOrientation.Vertical || (orientation == MenuOrientation.Responsive && containerHeight >= containerWidth))
                {
                    // Calculate item height based on container height, spacing, and margins for vertical layout
                    float availableHeight = containerHeight - (2 * sideMargin) - ((menuItems.Count - 1) * itemSpacing);
                    responsiveItemHeight = availableHeight / menuItems.Count;
                    responsiveItemWidth = itemWidth; // Keep original width for vertical responsive
                }
                else // Horizontal layout or responsive horizontal
                {
                    // Calculate item width based on container width, spacing, and margins for horizontal layout
                    float availableWidth = containerWidth - (2 * sideMargin) - ((menuItems.Count - 1) * itemSpacing);
                    responsiveItemWidth = availableWidth / menuItems.Count;
                    responsiveItemHeight = itemHeight; // Keep original height for horizontal responsive
                }
            }

            // Initialize position based on orientation
            float primaryPos = -sideMargin;
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
                
                // Positioning based on orientation
                var rect = ui.GetComponent<RectTransform>();
                
                if (orientation == MenuOrientation.Vertical || (orientation == MenuOrientation.Responsive && containerHeight >= containerWidth))
                {
                    // Vertical layout
                    rect.anchorMin = new Vector2(0, 1);
                    rect.anchorMax = new Vector2(1, 1);
                    rect.pivot = new Vector2(0.5f, 1);
                    rect.anchoredPosition = new Vector2(0, primaryPos);
                    
                    if (isResponsive || orientation == MenuOrientation.Responsive)
                        rect.sizeDelta = new Vector2(-2 * sideMargin, responsiveItemHeight);
                    else
                        rect.sizeDelta = new Vector2(-2 * sideMargin, itemHeight);
                    
                    primaryPos -= ((isResponsive || orientation == MenuOrientation.Responsive) ? responsiveItemHeight : itemHeight) + itemSpacing;
                }
                else // Horizontal layout
                {
                    rect.anchorMin = new Vector2(0, 0);
                    rect.anchorMax = new Vector2(0, 1);
                    rect.pivot = new Vector2(0, 0.5f);
                    rect.anchoredPosition = new Vector2(primaryPos, 0);
                    
                    if (isResponsive || orientation == MenuOrientation.Responsive)
                        rect.sizeDelta = new Vector2(responsiveItemWidth, -2 * sideMargin);
                    else
                        rect.sizeDelta = new Vector2(itemWidth, -2 * sideMargin);
                    
                    primaryPos += ((isResponsive || orientation == MenuOrientation.Responsive) ? responsiveItemWidth : itemWidth) + itemSpacing;
                }
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