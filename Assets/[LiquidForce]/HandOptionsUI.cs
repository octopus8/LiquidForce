using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace LiquidForce
{
    
    /// <summary>
    /// Handles hand options UI.
    /// </summary>
    public class HandOptionsUI : MonoBehaviour
    {
        
		#region Component Parameters

        [Header("Resources")]
        
        /// <summary>Prototype Hand Option GameObject.</summary>
        [Tooltip("Prototype Hand Option GameObject.")]
        [SerializeField] private GameObject prototypeOption;
        
        [SerializeField] private Image handThumbnail;
        
		#endregion
        


		#region Class Variables
        
        /// <summary>List of created options.</summary>
        private List<HandOption> options = new();
        
        private Sprite previousThumbnail;
        
		#endregion



		#region MonoBehaviour Functions

        /// <summary>
        /// MonoBehaviour lifecycle method; populates and initializes the list of available hand options.
        /// </summary>
        public void Start()
        {
            PopulateOptionList();

            HandOption selectedOption = options.Find(x => x.GetTitle() == Application.Instance.PlayerPreferences.handTypeTitle);
            selectedOption.GetComponent<Toggle>().Select();
            selectedOption.OnSelection();
        }

		#endregion


        /// <summary>
        /// Stores the current thumbnail and updates the hand thumbnail to the selected option's thumbnail.
        /// </summary>
        /// <param name="title"></param>
        public void OnPointerEnter(string title)
        {
            Debug.Log("POINTER ENTER!!!");
            HandOption selectedOption = options.Find(x => x.GetTitle() == title);
            previousThumbnail = handThumbnail.sprite;
            handThumbnail.sprite = selectedOption.GetThumbnail();
        }

        
        /// <summary>
        /// Restores the previous thumbnail when the pointer exits the option.
        /// </summary>
        /// <param name="title"></param>
        public void OnPointerExit(string title)
        {
            if (previousThumbnail != null)
            {
                handThumbnail.sprite = previousThumbnail;
            }
        }

        
        /// <summary>
        /// Sets the selected option as the active hand type and updates the thumbnail.
        /// </summary>
        /// <param name="title"></param>
        public void OnSelection(string title)
        {
            HandOption selectedOption = options.Find(x => x.GetTitle() == title);
            if (null != selectedOption)
            {
                handThumbnail.sprite = previousThumbnail = selectedOption.GetThumbnail();
            }
            else
            {
                Debug.LogError("Hand option not found: " + title);
            }
            
            Application.Instance.SetPlayerHands(title);
        }


		#region Helper Functions

        /// <summary>
        /// Populates the list of options.
        /// </summary>
        private void PopulateOptionList()
        {
            // Make sure the prototype option is not active.
            prototypeOption.SetActive(false);

            foreach (var handData in Application.Instance.HandData)
            {
                GameObject optionGO = Instantiate(prototypeOption, prototypeOption.transform.parent);
                var option = optionGO.GetComponent<HandOption>();
                option.Init(handData.title, handData.thumbnail);
                optionGO.SetActive(true);
                options.Add(option);
            }
        }

        #endregion

    }
}
