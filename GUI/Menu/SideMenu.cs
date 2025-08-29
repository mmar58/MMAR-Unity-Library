using UnityEngine;
using PrimeTween;
using EditorAttributes;

public class SideMenu : MonoBehaviour
{
    public enum MenuSide { Left, Right, Top, Bottom }
    [Header("Animation")]
    public MenuSide side = MenuSide.Left;
    public Vector2 startPoint;
    public Vector2 endPoint;
    public float duration = 0.5f;
    public bool shown = false;

    public RectTransform rectTransform;

    #region MonoBehaviour lifecycle methods

    void Awake()
    {
        Initialize();
        shown = false;
        rectTransform.anchoredPosition = shown ? startPoint : endPoint;
    }
    #endregion

    private void Initialize()
    {
        if(rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
    }


    #region Inspector methods
    [Button]
    void GrabXPosition()
    {
        Initialize();
        Vector2 size = rectTransform.rect.size;
        Vector2 anchored = rectTransform.anchoredPosition;
        switch (side)
        {
            case MenuSide.Left:
                if (shown)
                {
                    startPoint = anchored;
                    endPoint = new Vector2(anchored.x - size.x, anchored.y);
                }
                else
                {
                    endPoint = anchored;
                    startPoint = new Vector2(anchored.x + size.x, anchored.y);
                }
                break;
            case MenuSide.Right:
                if (shown)
                {
                    startPoint = anchored;
                    endPoint = new Vector2(anchored.x + size.x, anchored.y);
                }
                else
                {
                    endPoint = anchored;
                    startPoint = new Vector2(anchored.x - size.x, anchored.y);
                }
                break;
        }
    }
    [Button]
    void GrabYPosition()
    {
        Initialize();
        Vector2 size = rectTransform.rect.size;
        Vector2 anchored = rectTransform.anchoredPosition;
        switch (side)
        {
            case MenuSide.Top:
                if (shown)
                {
                    startPoint = anchored;
                    endPoint = new Vector2(anchored.x, anchored.y + size.y);
                }
                else
                {
                    endPoint = anchored;
                    startPoint = new Vector2(anchored.x, anchored.y - size.y);
                }
                break;
            case MenuSide.Bottom:
                if (shown)
                {
                    startPoint = anchored;
                    endPoint = new Vector2(anchored.x, anchored.y - size.y);
                }
                else
                {
                    endPoint = anchored;
                    startPoint = new Vector2(anchored.x, anchored.y + size.y);
                }
                break;
        }
    }
    #endregion
    public void ShowMenu(bool show)
    {
        if (Application.isPlaying)
        {
            shown = show;
            Vector2 target = shown ? startPoint : endPoint;
            Vector2 current = rectTransform.anchoredPosition;

            // Calculate how far along the animation is
            float totalDist = Vector2.Distance(startPoint, endPoint);
            float currentDist = Vector2.Distance(current, target);
            float t = totalDist > 0f ? currentDist / totalDist : 0f;
            float animDuration = duration * t;

            // Cancel any running tween
            //tween.Complete();
            Debug.Log($"Animating from {current} to {target} over {animDuration} seconds");
            // Animate to target position
            Tween.UIAnchoredPosition(rectTransform, target, animDuration > 0f ? animDuration : 0.01f);
        }
        else
        {
            shown = show;
            // In editor mode, just set the position directly
            rectTransform.anchoredPosition = show ? startPoint : endPoint;
        }
    }

    // Example usage: call this from a button
    [Button]
    public void ToggleMenu()
    {
        ShowMenu(!shown);
    }
}
