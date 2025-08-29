namespace MMAR.GUI.Menu 
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    public class MenuItemUI : MonoBehaviour
    {
        public TMP_Text label;
        public UnityEvent actions;

        public void SetData(MenuItem menuItem)
        {
            label.text = menuItem.name;
            actions = menuItem.actions;
        }

        public void OnClick()
        {
            actions?.Invoke();
        }
    }

}
