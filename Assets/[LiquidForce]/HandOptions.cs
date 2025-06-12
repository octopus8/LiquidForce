using System;
using UnityEngine;
using XRMultiplayer;

public class HandOptions : MonoBehaviour
{
    [SerializeField]
    private GameObject prototypeOption;
    
    [SerializeField]
    private PlayerAppearanceMenu appearanceMenu;


    public void Start()
    {
        // Make sure the prototype option is not active.
        prototypeOption.SetActive(false);

        foreach (var handData in appearanceMenu.HandData)
        {
            GameObject optionGO = Instantiate(prototypeOption, prototypeOption.transform.parent);
            var option = optionGO.GetComponent<HandOption>();
            option.title.text = handData.name;
            optionGO.SetActive(true);
        }
    }
    
}
