using UnityEngine;
using PrimeTween;
using EditorAttributes;

public class SideMenu : MonoBehaviour
{
    [Header("Animation")]
    public Vector2 startPoint;
    public Vector2 endPoint;
    public float duration = 0.5f;
    public bool shown = false;

    public RectTransform rectTransform;
    private Tween tween;

    #region MonoBehaviour lifecycle methods

    void Awake()
    {
        Initialize();
        rectTransform.anchoredPosition = shown ? endPoint : startPoint;
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
        startPoint.x = (shown ? rectTransform.anchoredPosition.x : 2* rectTransform.anchoredPosition.x);
        endPoint.x = (shown ? startPoint.x-rectTransform.rect.width : rectTransform.anchoredPosition.x);
    }
    [Button]
    void GrabYPosition()
    {
        Initialize();
        startPoint.y = rectTransform.anchoredPosition.y;
        endPoint.y = rectTransform.anchoredPosition.y;
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
            tween.Complete();

            // Animate to target position
            tween = Tween.UIAnchoredPosition(rectTransform, target, animDuration > 0f ? animDuration : 0.01f);
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
