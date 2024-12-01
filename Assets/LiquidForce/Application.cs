using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using WebXR;

namespace LiquidForce
{
    [RequireComponent(typeof(CameraFader))]
    [RequireComponent(typeof(DeviceTracking))]
    public class Application : MonoBehaviour
    {

        [DllImport("__Internal")]
        private static extern void WebXROnApplicationReady();
        
        public static Application Instance;
        
        [HideInInspector]
        public CameraFader cameraFader;
        
        [HideInInspector]
        public DeviceTracking deviceTracking;

        public Action<WebXRState> OnXRChange;
        
        
        private void Awake()
        {
            Instance = this;

            cameraFader = GetComponent<CameraFader>();
            deviceTracking = GetComponent<DeviceTracking>();
        }

        private void Start()
        {
            cameraFader.SetCameraFadedOut();

            WebXRManager.OnXRChange += OnXRChanged;
        }

        private void OnXRChanged(WebXRState state, int viewsCount, Rect leftRect, Rect rightRect)
        {
            OnXRChange?.Invoke(state);
        }

        public void OnApplicationReady()
        {
            WebXROnApplicationReady();
        }
        
    }
}
