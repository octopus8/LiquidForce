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
}
