using UnityEngine;

using Microphone = Estrada.Microphone;


public class Dev : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Microphone.GetPosition(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //Write a function that returns the app name. Obtain the app name from the project settings.
    public string GetAppName()
    {
        return Application.productName;
    }
    
}
