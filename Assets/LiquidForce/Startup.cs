using System;
using System.Collections;
using UnityEngine;
using WebXR;

namespace LiquidForce
{
    public class Startup : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR            
            Application.Instance.OnXRChange += OnXRChange;
#else
            StartCoroutine(FadeInDelayed());
#endif
        }



        private void OnXRChange(WebXRState state)
        {
            if (state != WebXRState.NORMAL)
            {
                StartCoroutine(FadeInDelayed());
            }
        }

        IEnumerator FadeInDelayed()
        {
            yield return new WaitForSeconds(1);
            Application.Instance.cameraFader.FadeCameraIn();
            Application.Instance.OnXRChange -= OnXRChange;
            Destroy(gameObject);
        }
    }
    
}

