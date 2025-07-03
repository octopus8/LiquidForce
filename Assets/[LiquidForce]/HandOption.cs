using TMPro;
using UnityEngine;


namespace LiquidForce
{
    public class HandOption : MonoBehaviour
    {
        [SerializeField]
        private HandOptionsUI handOptionsUI;
    
        [Header("Components")]
        public TextMeshProUGUI title;

        public void OnSelection()
        {
            Application.Instance.SetPlayerHands(title.text);
        }
    }
}
