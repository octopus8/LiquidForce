using System;
using System.Collections;
using UnityEngine;

namespace LiquidForce
{
    [RequireComponent(typeof(CameraFader))]
    [RequireComponent(typeof(DeviceTracking))]
    public class Application : MonoBehaviour
    {
        public static Application Instance;
        
        [HideInInspector]
        public CameraFader cameraFader;
        
        [HideInInspector]
        public DeviceTracking deviceTracking;

        
        
        private void Awake()
        {
            Instance = this;

            cameraFader = GetComponent<CameraFader>();
            deviceTracking = GetComponent<DeviceTracking>();
        }

        private void Start()
        {
            StartCoroutine(StartDelayed());
        }


        IEnumerator StartDelayed()
        {
            yield return new WaitForSeconds(5);
            Debug.Log("Fade");
            cameraFader.SetCameraFadedOut();
        }
    }
}
