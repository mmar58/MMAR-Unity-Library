namespace MMAR.GUI.PageSystem
{

    using System.Collections.Generic;
    using UnityEngine;

    public class PageController : MonoBehaviour
    {
        public List<PageData> pages;
        public bool enablePageHistory = false;
        public int defaultPageIndex = 0;
        public bool debug = false; // Debug flag
        [Header("Page history")]
        [HideInInspector]
        public PageData currentPage;
        [HideInInspector]
        public PageData previousPage;
        [HideInInspector]
        public List<PageData> pageHistory;
        [HideInInspector]
        public bool isInitialized = false;

        public virtual void Awake()
        {
            Initialized();
        }
        public virtual void Initialized()
        {
            if (debug) Debug.Log("[PageController] Awake called");
            if (pages == null || pages.Count == 0)
            {
                Debug.LogError("No pages defined in PageController.");
                return;
            }
            if (defaultPageIndex < 0 || defaultPageIndex >= pages.Count)
            {
                Debug.LogError("Default page index out of range: " + defaultPageIndex);
                return;
            }
            if (debug) Debug.Log($"[PageController] Showing default page at index {defaultPageIndex}");
            ShowPage(defaultPageIndex);
            isInitialized = true;
        }
        public virtual void ShowPage(string pageName)
        {
            if (debug) Debug.Log($"[PageController] ShowPage called with pageName: {pageName}");
            if (enablePageHistory && currentPage != null)
            {
                if (debug) Debug.Log($"[PageController] Adding currentPage '{currentPage?.name}' to history");
                pageHistory.Add(currentPage);
            }
            for (int i = 0; i < pages.Count; i++)
            {
                if (pages[i].name == pageName)
                {
                    if (debug) Debug.Log($"[PageController] Activating page '{pages[i].name}' at index {i}");
                    currentPage = pages[i];
                    pages[i].Active = true;
                    OnShowingPage(i, pages[i]);
                }
                else
                {
                    if (pages[i].Active)
                    {
                        if (debug) Debug.Log($"[PageController] Deactivating previous page '{pages[i].name}' at index {i}");
                        previousPage = pages[i];
                    }
                    pages[i].Active = false;
                }
            }
        }
        public virtual void ShowPage(int pageIndex)
        {
            if (debug) Debug.Log($"[PageController] ShowPage called with pageIndex: {pageIndex}");
            if (pageIndex < 0 || pageIndex >= pages.Count)
            {
                Debug.Log("Page index out of range: " + pageIndex);
                return;
            }
            if (currentPage != null)
            {
                if (enablePageHistory)
                {
                    if (debug) Debug.Log($"[PageController] Adding currentPage '{currentPage?.name}' to history");
                    pageHistory.Add(currentPage);
                }
                if (debug) Debug.Log($"[PageController] Setting previousPage to '{currentPage?.name}'");
                previousPage = currentPage;
            }

            currentPage = pages[pageIndex];
            if (debug) Debug.Log($"[PageController] Activating page '{currentPage.name}' at index {pageIndex}");
            OnShowingPage(pageIndex, currentPage);
            currentPage.Active = true;
            for (int i = 0; i < pages.Count; i++)
            {
                if (i != pageIndex)
                {
                    if (pages[i].Active && debug) Debug.Log($"[PageController] Deactivating page '{pages[i].name}' at index {i}");
                    pages[i].Active = false;
                }
            }
        }
        public virtual void OnShowingPage(int pageIndex,PageData pageData)
        {
            if (debug) Debug.Log($"[PageController] OnShowingPage: Showing page: {pageData.name} at index {pageIndex}");
            // Override this method to handle additional logic when a page is shown
            Debug.Log($"Showing page: {pageData.name} at index {pageIndex}");
        }
    }
    [System.Serializable]
    public class PageData
    {
        /// <summary>
        /// Page name, used to identify the page.
        /// </summary>
        public string name;
        /// <summary>
        /// Page GameObject, which should be set active when the page is shown.
        /// </summary>
        public GameObject div;
        public bool isActive = false;

        public bool Active
        {
            get => isActive;
            set
            {
                isActive = value;
                div.SetActive(value);
            }
        }
    }
}