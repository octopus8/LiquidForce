using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using XRMultiplayer;
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
        
        [SerializeField]
        private OfflinePlayerAvatar offlinePlayerAvatar;
        
        /// <summary>The hand data.</summary>
        [Tooltip("The hand data.")]
        [field: SerializeField]
        public HandData[] HandData { get; private set; }
        
        private string appPlayerPrefsKey = "appPlayerPrefs";
        
        public AppPlayerPrefs PlayerPreferences { get; private set; } = new AppPlayerPrefs();
        
        [Serializable]
        public struct AppPlayerPrefs
        {
            public string username;
            public string handTypeTitle;

            public AppPlayerPrefs(string username, string handTypeTitle)
            {
                this.username = "Anonymous";
                this.handTypeTitle = Application.Instance.HandData?[0].title;
            }
        }
        
        /// <summary>
        /// Monobehaviour lifecycle method; references are stored and variables are initialized. The GameObject is set to "Don't Destroy On Load".
        /// </summary>
        private void Awake()
        {
            Instance = this;

            cameraFader = GetComponent<CameraFader>();
            deviceTracking = GetComponent<DeviceTracking>();
            
            if (PlayerPrefs.HasKey(appPlayerPrefsKey))
            {
                string json = PlayerPrefs.GetString(appPlayerPrefsKey);
                PlayerPreferences = JsonUtility.FromJson<AppPlayerPrefs>(json);
            }            
            
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            offlinePlayerAvatar.OnMicrophonePermissionsCompleted.AddListener(OnApplicationReady);
            
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
