using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
#if UNITY_WEBGL
using WebXR;
#endif

namespace LiquidForce
{
    [RequireComponent(typeof(CameraFader))]
    [RequireComponent(typeof(DeviceTracking))]
    public class Application : MonoBehaviour
    {
        
        [DllImport("__Internal")]
        private static extern void WebXROnApplicationReady();
        
        public static Application Instance;
        
        public string LogPrepend = "<color=#e5f73b>[Application]</color> ";
        
        
        [HideInInspector]
        public CameraFader cameraFader;
        
        [HideInInspector]
        public DeviceTracking deviceTracking;

#if UNITY_WEBGL
        public Action<WebXRState> OnXRChange;
#endif        
        
        private void Awake()
        {
            Instance = this;

            cameraFader = GetComponent<CameraFader>();
            deviceTracking = GetComponent<DeviceTracking>();
        }

        private void Start()
        {
            cameraFader.SetCameraFadedOut();
#if UNITY_WEBGL
            WebXRManager.OnXRChange += OnXRChanged;
#endif
        }

#if UNITY_WEBGL
        
        private void OnXRChanged(WebXRState state, int viewsCount, Rect leftRect, Rect rightRect)
        {
            OnXRChange?.Invoke(state);
        }
#endif
        
        public void OnApplicationReady()
        {
#if UNITY_WEBGL && !UNITY_EDITOR            
            WebXROnApplicationReady();
#endif
        }
        
    }
}
