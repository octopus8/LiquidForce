using System.Collections.Generic;
using UnityEngine;
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
        
        /// <summary>The hand data.</summary>
        [Tooltip("The hand data.")]
        [SerializeField] private HandData[] handData;
        
#endregion
        


#region Class Variables
        
        /// <summary>List of created options.</summary>
        private List<HandOption> options = new();
        
#endregion



#region MonoBehaviour Functions

        /// <summary>
        /// MonoBehaviour lifecycle method; populates and initializes the list of available hand options.
        /// </summary>
        public void Start()
        {
            PopulateOptionList();

            InitCurrentSelection();
        }

#endregion



#region Helper Functions

        /// <summary>
        /// Populates the list of options.
        /// </summary>
        private void PopulateOptionList()
        {
            // Make sure the prototype option is not active.
            prototypeOption.SetActive(false);

            foreach (var handData in handData)
            {
                GameObject optionGO = Instantiate(prototypeOption, prototypeOption.transform.parent);
                var option = optionGO.GetComponent<HandOption>();
                option.title.text = handData.name;
                optionGO.SetActive(true);
                options.Add(option);
            }
        }

        
        /// <summary>
        /// Initializes the current selection.
        /// </summary>
        private void InitCurrentSelection()
        {
            string currentHandType = Application.Instance.PlayerSettings.HandType;
            HandOption selectedOption = options.Find(x => x.title.text == currentHandType);
            if (null != selectedOption)
            {
                selectedOption.GetComponent<Toggle>().isOn = true;
            }
            else
            {
                options[0].GetComponent<Toggle>().isOn = true;
            }
        }
        
#endregion

    }
}
