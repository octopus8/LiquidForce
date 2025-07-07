using System.Collections;
using UnityEngine;
#if UNITY_WEBGL
using WebXR;
#endif

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


#if UNITY_WEBGL
        private void OnXRChange(WebXRState state)
        {
            if (state != WebXRState.NORMAL)
            {
                StartCoroutine(FadeInDelayed());
            }
        }
#endif
        
        /// <summary>
        /// Waits for a period of time, then fades the camera in. This is used at startup to give the Application
        /// a moment to settle once ready before fading the scene in.
        /// </summary>
        /// <returns></returns>
        IEnumerator FadeInDelayed()
        {
            yield return new WaitForSeconds(1);
            _ = LiquidForce.Application.Instance.CameraFader.FadeCameraIn();
#if UNITY_WEBGL

            Application.Instance.OnXRChange -= OnXRChange;
#endif
        }
    }
    
}

