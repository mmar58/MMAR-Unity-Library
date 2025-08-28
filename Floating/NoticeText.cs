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
            textField.text = notice;
            if (ColorUtility.TryParseHtmlString(color, out Color parsedColor))
            {
                textField.color = parsedColor;
            }
            else
            {
                textField.color = Color.black;
            }
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