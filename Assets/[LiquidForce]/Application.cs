using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Hands.Samples.VisualizerSample;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using XRMultiplayer;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
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

        #region Editor Variables
        
        [SerializeField]
        private GameObject xrOrigin;
        
        
        /// <summary>The `OfflinePlayerAvatar` component. This is used to detect when the application is ready.</summary>
        [SerializeField]
        private OfflinePlayerAvatar offlinePlayerAvatar;
        
        /// <summary>An array of `HandData` ScriptableObjects that hold hand data.</summary>
        [Tooltip("An array of `HandData` ScriptableObjects that hold hand data.")]
        [field: SerializeField]
        public AppHandData[] HandData { get; private set; }

        [Header("Internal References")]

        /// <summary>The XR Input Modality Manager.</summary>
        [SerializeField]
        [Tooltip("The XR Input Modality Manager.")]
        private XRInputModalityManager xrInputModalityManager;

        /// <summary>The Hand Visualizer.</summary>
        [SerializeField]
        [Tooltip("The Hand Visualizer.")]
        private HandVisualizer handParent;
        
        [field: SerializeField]
        public SceneData[] Locations {  get; private set; }

        #endregion

        
        #region Public Properties
        
        /// <summary>The Singleton instance of the component.</summary>
        public static Application Instance;
        
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
        
        /// <summary>String used to prepend application log messages.</summary>
        public const string LogPrepend = "<color=#e5f73b>[Application]</color> ";
        
        #endregion


        #region Private Variables

        /// <summary>The key for player player prefs.</summary>
        private string appPlayerPrefsKey = "appPlayerPrefs";
        
        private bool isTrackingHands = false;
        
        public string CurrentLocation { private set; get; } = string.Empty;

        #endregion

        
        #region MonoBehaviour Lifecycle
        
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
            
            // Init preferences.
            InitPreferences();
            
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

            // Set the player hands based on the player preferences.
            SetPlayerHands(PlayerPreferences.handTypeTitle);

            xrInputModalityManager.trackedHandModeStarted.AddListener(TrackedHandModeStarted);
            xrInputModalityManager.motionControllerModeStarted.AddListener(TrackedHandModeEnded);

            SetLocation(Locations[0].sceneName);

#if UNITY_WEBGL
            // Add a listener to get when microphone permissions have been completed.
            offlinePlayerAvatar.OnMicrophonePermissionsCompleted.AddListener(OnApplicationReady);

            // Add an "on XR changed" listener.
            WebXRManager.OnXRChange += OnXRChanged;
#endif
        }


        private void OnDestroy()
        {
            xrInputModalityManager.trackedHandModeStarted.RemoveListener(TrackedHandModeStarted);
            xrInputModalityManager.trackedHandModeStarted.RemoveListener(TrackedHandModeEnded);
        }

        #endregion

        
        #region Public Methods

        public void SetPlayerHands(string handTypeTitle)
        {
            // Get the player hands based on the hand type title.
            AppHandData currentHandData = null;
            if (HandData != null)
            {
                foreach (var handData in HandData)
                {
                    if (handData.title == handTypeTitle)
                    {
                        currentHandData = handData;
                        break;
                    }
                }
            }
            if (null == currentHandData)
            {
                return;
            }

            // Instantiate the lefthand prefab.
            if (xrInputModalityManager.leftHand != null)
            {
                Destroy(xrInputModalityManager.leftHand);
            }
            xrInputModalityManager.leftHand = Instantiate(currentHandData.lefHandPrefab, handParent.transform);
            handParent.LeftHandInteractionVisual = xrInputModalityManager.leftHand.GetComponent<HandComponents>().InteractionVisual;
            xrInputModalityManager.leftHand.SetActive(isTrackingHands);
            
            // Instantiate the righthand prefab.
            if (xrInputModalityManager.rightHand != null)
            {
                Destroy(xrInputModalityManager.rightHand);
            }
            xrInputModalityManager.rightHand = Instantiate(currentHandData.rightHandPrefab, handParent.transform);
            handParent.RightHandInteractionVisual = xrInputModalityManager.rightHand.GetComponent<HandComponents>().InteractionVisual;
            xrInputModalityManager.rightHand.SetActive(isTrackingHands);
            
            // Update the player preferences.
            var prefs = PlayerPreferences;
            prefs.handTypeTitle = currentHandData.title;
            PlayerPreferences = prefs;
        }

        
        
        public void SetLocation(string locationName)
        {
            // Find the scene data for the given location name.
            SceneData sceneData = Array.Find(Locations, x => x.sceneName == locationName);
            if (sceneData.Equals(default(SceneData)))
            {
                Debug.LogError(LogPrepend + "Location not found: " + locationName);
                return;
            }

            // Load the scene async and position the xrOrigin after loading.
            var asyncOp = SceneManager.LoadSceneAsync(sceneData.scenePath, LoadSceneMode.Additive);
            asyncOp.completed += (op) =>
            {
                var success = false;
                // Find the loaded scene by path
                var loadedScene = SceneManager.GetSceneByPath("Assets/" + sceneData.scenePath + ".unity");
                if (loadedScene.IsValid())
                {
                    GameObject[] rootObjects = loadedScene.GetRootGameObjects();
                    foreach (var go in rootObjects)
                    {
                        if (go.name == "Start Transform")
                        {
                            xrOrigin.transform.position = go.transform.position;
                            xrOrigin.transform.rotation = go.transform.rotation;
                            success = true;
                            break;
                        }
                    }
                }

                if (success)
                {
                    CurrentLocation = locationName;
                }
                else
                {
                    Debug.LogError(LogPrepend + "Failed to find 'Start Transform' in the loaded scene: " + sceneData.sceneName);
                }
            };
        }

        #endregion

        
        #region Helper Methods

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

        private void TrackedHandModeStarted()
        {
            isTrackingHands = true;
        }

        private void TrackedHandModeEnded()
        {
            isTrackingHands = false;
        }
        
        
        #endregion

        
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
                this.handTypeTitle = handTypeTitle;
            }
        }

        [Serializable]
        public struct SceneData
        {
            public string sceneName;
            public string scenePath;
        }
        
        #endregion

    }
}
