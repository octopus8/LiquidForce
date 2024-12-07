using System;
using System.Collections;
using UnityEngine;
#if UNITY_WEBGL
using WebXR;
#endif

namespace LiquidForce
{
    public class Startup : MonoBehaviour
    {
        [SerializeField]
        protected GameObject playerGameObject;

        [SerializeField]
        protected Transform startTransform;
        
        [SerializeField]
        protected GameObject logConsole;

        [SerializeField]
        protected bool showLogConsole = true;
        
        private bool isPlayerInited = false;
        

        private void Awake()
        {
            logConsole.gameObject.SetActive(showLogConsole);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR            
            Application.Instance.OnXRChange += OnXRChange;
#else
            StartCoroutine(FadeInDelayed());
#endif
        }

        private int fixedFrameCount = 0;
        
        private void FixedUpdate()
        {
            if (isPlayerInited || ++fixedFrameCount < 2)
            {
                return;
            }
            
            playerGameObject.transform.SetPositionAndRotation(startTransform.position, startTransform.rotation);
            logConsole.transform.SetPositionAndRotation(playerGameObject.transform.position, playerGameObject.transform.rotation);
            isPlayerInited = true;
            
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
        
        IEnumerator FadeInDelayed()
        {
            yield return new WaitForSeconds(1);
            Application.Instance.cameraFader.FadeCameraIn();
#if UNITY_WEBGL
            
            Application.Instance.OnXRChange -= OnXRChange;
#endif
        }
    }
    
}

