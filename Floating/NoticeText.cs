namespace MMAR.Floating
{
    using TMPro;
    using UnityEngine;
    public class NoticeText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textField;
        [SerializeField] GameObject noticePanel;
        float lastShown = -100;
        public float showingDuration = 2;
        public bool setThisOneAsInstance = true;
        public bool autoHide = true;
        [Header("Default Color")]
        public Color infoColor = Color.black;
        public Color warningColor = Color.yellow;
        public Color errorColor = Color.red;

        public static NoticeText instance;

        private void Awake()
        {
            if (setThisOneAsInstance)
            {
                instance = this;
            }
            ShowGameObject();
        }
        public void SetThisOneAsInstance()
        {
            setThisOneAsInstance = true;
            instance = this;
        }
        public void ShowNotice(string notice, string color = "#000000")
        {
            //Debug.Log("[NoticeText] ShowNotice: " + notice);
            
            if (ColorUtility.TryParseHtmlString(color, out Color parsedColor))
            {
                ShowNotice(notice, parsedColor);
            }
            else
            {
                ShowNotice(notice, Color.black);
            }
        }
        public void ShowNotice(string notice, NoticeType type)
        {
            switch(type)
            {
                case NoticeType.Info:
                    ShowNotice(notice, infoColor);
                    break;
                case NoticeType.Warning:
                    ShowNotice(notice, warningColor);
                    break;
                case NoticeType.Error:
                    ShowNotice(notice, errorColor);
                    break;
                default:
                    ShowNotice(notice, infoColor);
                    break;
            }
        }
        public void ShowNotice(string notice,Color color)
        {
            textField.text = notice;
            textField.color = color;
            lastShown = Time.time;
            ShowGameObject();
        }
        void ShowGameObject()
        {
            if (noticePanel != null)
            {
                noticePanel.SetActive(true);
            }
            else
            {
                textField.gameObject.SetActive(true);
            }
        }
        public void HideNotice()
        {
            Debug.Log("[NoticeText] HideNotice "+Time.time);
            if (noticePanel != null)
            {
                noticePanel.SetActive(false);
            }
            else
            {
                textField.gameObject.SetActive(false);
            }
        }
        void FixedUpdate()
        {
            if (autoHide && Time.time - lastShown > showingDuration)
            {
                HideNotice();
            }
        }
    }

}

public enum NoticeType
{
    Info,
    Warning,
    Error
}