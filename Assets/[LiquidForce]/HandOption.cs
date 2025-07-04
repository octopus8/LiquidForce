using TMPro;
using UnityEngine;


namespace LiquidForce
{
    
    public class HandOption : MonoBehaviour
    {
        [Header("Resources")]
        
        [SerializeField] private HandOptionsUI handOptionsUI;
        
        [SerializeField] private TextMeshProUGUI title;
        
        private Sprite thumbnail;

        public void Init(string title, Sprite thumbnail)
        {
            this.title.text = title;
            this.thumbnail = thumbnail;
        }
        
        public string GetTitle()
        {
            return title.text;
        }
        
        public Sprite GetThumbnail()
        {
            return thumbnail;
        }
        
        public void OnPointerEnter()
        {
            handOptionsUI.OnPointerEnter(title.text);
        }

        public void OnPointerExit()
        {
            handOptionsUI.OnPointerExit(title.text);
        }
        

        public void OnSelection()
        {
            handOptionsUI.OnSelection(title.text);
        }
    }
}
