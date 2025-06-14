using UnityEngine;

public class LocationPanel : MonoBehaviour
{ 
    [SerializeField]
    private GameObject prototypeOption;
    
    [SerializeField]
    private LocationPanelOptionData[] locations;
    
    
    void Start()
    {
        // Make sure the prototype option is not active.
        prototypeOption.SetActive(false);
        
        GameObject optionGO = Instantiate(prototypeOption, prototypeOption.transform.parent);
        LocationsPanelOption option = optionGO.GetComponent<LocationsPanelOption>();
        option.title.text = locations[0].sceneName;
        optionGO.SetActive(true);
    }

}
