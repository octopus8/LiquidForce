using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XRMultiplayer;

namespace LiquidForce
{


    public class LocationPanel : MonoBehaviour
    {
        [SerializeField] private GameObject prototypeOption;
        
        [SerializeField] private PlayerOptions playerOptions;

        private List<LocationsPanelOption> options = new();


        void Start()
        {
            PopulateOptionList();

            LocationsPanelOption selectedOption =
                options.Find(x => x.GetTitle() == Application.Instance.CurrentLocation);
            selectedOption.GetComponent<Toggle>().Select();
            selectedOption.OnSelection();
        }

        private void PopulateOptionList()
        {
            options.Clear();
            // Make sure the prototype option is not active.
            prototypeOption.SetActive(false);
            foreach (var location in Application.Instance.Locations)
            {
                // Create a new option GameObject
                GameObject optionGO = Instantiate(prototypeOption, prototypeOption.transform.parent);
                // Get the LocationsPanelOption component
                LocationsPanelOption option = optionGO.GetComponent<LocationsPanelOption>();
                // Initialize the option with the location title
                option.Init(location.sceneName);
                optionGO.SetActive(true);
                // Add the option to the list
                options.Add(option);
            }
        }

        public void OnSelection(string title)
        {
            LocationsPanelOption selectedOption = options.Find(x => x.GetTitle() == title);
            if (null == selectedOption)
            {
                Debug.LogError("Hand option not found: " + title);
                return;
            }
            
            Application.Instance.SetLocation(title);
//            playerOptions.ToggleMenu();
        }
        
        private void SetLocation(string title)
        {
            Application.Instance.SetLocation(title);
            playerOptions.ToggleMenu();
        }

    }
}
