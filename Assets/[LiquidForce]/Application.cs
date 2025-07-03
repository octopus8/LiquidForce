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
    /// <summary>
    /// The main application class.
    /// </summary>
    /// <remarks>
    /// - Application Ready
    ///   - To avoid entering VR in web, then having the microphone permissions return the view to 2D, the app waits until
    /// the microphone permissions process has completed before allowing the user to enter VR.
    /// </remarks>
    [RequireComponent(typeof(CameraFader))]
    [RequireComponent(typeof(DeviceTracking))]
    public class Application : MonoBehaviour
    {
        /// <summary>Dispatches the 'onApplicationReady' event to JavaScript.</summary>
#if UNITY_WEBGL        
        [DllImport("__Internal")]
        private static extern void WebXROnApplicationReady();
#endif        

        /// <summary>The `OfflinePlayerAvatar` component. This is used to detect when the application is ready.</summary>
        [SerializeField]
        private OfflinePlayerAvatar offlinePlayerAvatar;
        
        /// <summary>An array of `HandData` ScriptableObjects that hold hand data.</summary>
        [Tooltip("An array of `HandData` ScriptableObjects that hold hand data.")]
        [field: SerializeField]
        public AppHandData[] HandData { get; private set; }
        
        /// <summary>The Singleton instance of the component.</summary>
        public static Application Instance;

        /// <summary>String used to prepend application log messages.</summary>
        public const string LogPrepend = "<color=#e5f73b>[Application]</color> ";
        

        [Header("Internal References")]
        
        
#if UNITY_WEBGL
        /// <summary>Action callbacks called upon XR change.</summary>
        public Action<WebXRState> OnXRChange;
#endif        
        
        /// <summary>Player preferences.</summary>
        public AppPlayerPrefs PlayerPreferences { get; private set; } = new AppPlayerPrefs();

        /// <summary>The Camera Fader.</summary>
        public CameraFader CameraFader { get; private set; }
        
        /// <summary>Provides functionality to allow objects to follow tracked devices.</summary>
        public DeviceTracking DeviceTracking { get; private set; }

        /// <summary>The key for player player prefs.</summary>
        private string appPlayerPrefsKey = "appPlayerPrefs";

        
        
        /// <summary>
        /// Monobehaviour lifecycle method; references are stored and variables are initialized. The GameObject is set to "Don't Destroy On Load".
        /// </summary>
        private void Awake()
        {
            // Store the reference to the Singleton instance.
            Instance = this;

            // Get references.
            CameraFader = GetComponent<CameraFader>();
            DeviceTracking = GetComponent<DeviceTracking>();
            
            // Set the object as persistant.
            DontDestroyOnLoad(gameObject);
        }

        
        /// <summary>
        /// Monobehaviour lifecycle method; 
        /// </summary>
        private void Start()
        {
            // Set the camera as faded out.
            CameraFader.SetCameraFadedOut();

            // Init preferences.
            InitPreferences();
            
#if UNITY_WEBGL
            // Add a listener to get when microphone permissions have been completed.
            offlinePlayerAvatar.OnMicrophonePermissionsCompleted.AddListener(OnApplicationReady);

            // Add an "on XR changed" listener.
            WebXRManager.OnXRChange += OnXRChanged;
#endif
        }
        
        
        /// <summary>
        /// Loads player preferences from PlayerPrefs if available, otherwise initializes with default values.
        /// </summary>
        private void InitPreferences()
        {
            // Load the player preferences.
            if (PlayerPrefs.HasKey(appPlayerPrefsKey))
            {
                string json = PlayerPrefs.GetString(appPlayerPrefsKey);
                PlayerPreferences = JsonUtility.FromJson<AppPlayerPrefs>(json);
            }
            else
            {
                // Initialize with default values.
                PlayerPreferences = new AppPlayerPrefs("Anonymous", HandData?[0].title);
            }
        }

        
#region WebGL Functions        
#if UNITY_WEBGL
        /// <summary>
        /// Callback called upon the application being ready, this method calls `WebXROnApplicationReady`.
        /// </summary>
        private void OnApplicationReady()
        {
#if !UNITY_EDITOR            
            WebXROnApplicationReady();
#endif
        }


        /// <summary>
        /// Callback called upon XR state changes, this method invokes the `OnXRChange` action callbacks.
        /// </summary>
        private void OnXRChanged(WebXRState state, int viewsCount, Rect leftRect, Rect rightRect)
        {
            OnXRChange?.Invoke(state);
        }
#endif
#endregion


#region Data Structures

        /// <summary>
        /// The Player Preferences.
        /// </summary>
        [Serializable]
        public struct AppPlayerPrefs
        {
            /// <summary>Username.</summary>
            public string username;
            
            /// <summary>
            /// Title of the type of hands used.
            /// </summary>
            public string handTypeTitle;

            /// <summary>
            /// Constructor; initializes values to default values.
            /// </summary>
            public AppPlayerPrefs(string username, string handTypeTitle)
            {
                this.username = "Anonymous";
                this.handTypeTitle = Instance.HandData?[0].title;
            }
        }
        
#endregion

    }
}
