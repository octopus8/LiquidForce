using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VRLogConsole.Scripts.Presenters.Config;
using WebXR;

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
/*
        private int fixedFrameCount = 0;
        
        private void FixedUpdate()
        {
            if (isPlayerInited || ++fixedFrameCount < 20)
            {
                return;
            }
            
            playerGameObject.transform.SetPositionAndRotation(startTransform.position, startTransform.rotation);
            logConsole.GetComponent<ConfigurationPresenter>().configuration.UpdateLookAtPlayer(true);
            isPlayerInited = true;
            
        }
*/


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
            Application.Instance.cameraFader.FadeCameraIn().Forget();
            Application.Instance.OnXRChange -= OnXRChange;
        }
    }
    
}

