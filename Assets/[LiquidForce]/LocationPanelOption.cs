using UnityEngine;
using TMPro;

namespace LiquidForce
{



    public class LocationsPanelOption : MonoBehaviour
    {
        [SerializeField] private LocationPanel locationPanel;


        [SerializeField] private TextMeshProUGUI title;

        
        public void Init(string title)
        {
            this.title.text = title;
        }

        public string GetTitle()
        {
            return title.text;
        }


        public void OnSelection()
        {
            locationPanel.OnSelection(title.text);
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}