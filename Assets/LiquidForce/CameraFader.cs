using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;


namespace LiquidForce {

    /// <summary>
    /// Provides camera fading functionality.
    /// </summary>
    public class CameraFader : MonoBehaviour {

        /// <summary>The default fade duration in seconds.</summary>
        protected const float DefaultFadeDurationSeconds = 0.25f;


        /// <summary>The fade material.</summary>
        protected static Material fadeMaterial = null;

        protected GameObject cameraFaderRoot;


        /// <summary>Token that allows for the fade animation to be canceled.</summary>
        static protected CancellationTokenSource animCancel = null;



        /// <summary>
        /// Creates objects, meshes, materials for the camera fader.
        /// </summary>
        private void Awake() {

            // Create objects.
            cameraFaderRoot = new GameObject("CameraFader");
            GameObject cameraFaderOffset = new GameObject("Offset", typeof(MeshRenderer));
            cameraFaderOffset.transform.parent = cameraFaderRoot.transform;
            cameraFaderOffset.transform.localPosition = new Vector3(0, 0, 0.11f);
            MeshFilter meshFilter = cameraFaderOffset.AddComponent<MeshFilter>();
            Mesh mesh = meshFilter.mesh;

            // Define verticies.
            float width = 1f;
            float height = 1f;
            Vector3[] vertices = new Vector3[4];
            vertices[0] = new Vector3(-width * .5f, height * .5f, 0);
            vertices[1] = new Vector3(width * .5f, height * .5f, 0);
            vertices[2] = new Vector3(width * .5f, -height * .5f, 0);
            vertices[3] = new Vector3(-width * .5f, -height * .5f, 0);

            // Define triangles.
            int[] triangles = new int[]
            {
                0, 1, 2,
                0, 2, 3,
            };

            // Build mesh.
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.Optimize();

            // Create the material.
            fadeMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            fadeMaterial.SetFloat("_Surface", 1.0f);
            fadeMaterial.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            fadeMaterial.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            fadeMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            fadeMaterial.SetOverrideTag("RenderType", "Transparent");
            fadeMaterial.SetFloat("_ZWrite", 0.0f);
            fadeMaterial.SetFloat("_ZTest", 0.0f);
            fadeMaterial.renderQueue = (int)RenderQueue.Transparent;
            fadeMaterial.renderQueue += fadeMaterial.HasProperty("_QueueOffset") ? (int)fadeMaterial.GetFloat("_QueueOffset") : 0;
            fadeMaterial.SetShaderPassEnabled("ShadowCaster", false);
            fadeMaterial.color = new Color(0, 0, 0, 1.0f);
 
            // Set the material.
            cameraFaderOffset.GetComponent<Renderer>().material = fadeMaterial;
        }

        private void Start()
        {
            // Set the object to follow the head.
            Application.Instance.deviceTracking.AddHeadTarget(cameraFaderRoot);
        }


        /// <summary>
        /// Called to set the camera fader completely invisible.
        /// </summary>
        public void SetCameraFadedIn() {
            if (animCancel != null) {
                animCancel.Cancel();
                animCancel.Dispose();
                animCancel = null;
            }
            cameraFaderRoot.SetActive(false);
            Color color = fadeMaterial.color;
            color.a = 0.0f;
            fadeMaterial.color = color;
        }



        /// <summary>
        /// Called to set the camera completely faded out.
        /// </summary>
        public void SetCameraFadedOut() {
            if (animCancel != null) {
                animCancel.Cancel();
                animCancel.Dispose();
                animCancel = null;
            }
            cameraFaderRoot.SetActive(true);
            Color color = fadeMaterial.color;
            color.a = 1.0f;
            fadeMaterial.color = color;
        }



        /// <summary>
        /// Called to fade the camera out.
        /// </summary>
        /// <param name="durationSeconds"></param>
        /// <returns></returns>
        public async UniTask FadeCameraOut(float durationSeconds = DefaultFadeDurationSeconds) {
            await DoFade(durationSeconds, true);
        }



        /// <summary>
        /// Called to fade the camera in.
        /// </summary>
        /// <param name="durationSeconds"></param>
        /// <returns></returns>
        public async UniTask FadeCameraIn(float durationSeconds = DefaultFadeDurationSeconds) {
            await DoFade(durationSeconds, false);
        }



        /// <summary>
        /// Called by public fade methods to actually perform the fade.
        /// </summary>
        /// <param name="durationSeconds"></param>
        /// <param name="isFadeOut"></param>
        /// <returns></returns>
        protected async UniTask DoFade(float durationSeconds, bool isFadeOut) {
            if (animCancel != null) {
                animCancel.Cancel();
                animCancel.Dispose();
            }
            animCancel = new();
            Color color = fadeMaterial.color;
            color.a = isFadeOut ? 1.0f : 0.0f;
            cameraFaderRoot.SetActive(true);
            await fadeMaterial.DOColor(color, durationSeconds).WithCancellation(animCancel.Token);
            cameraFaderRoot.SetActive(isFadeOut);
        }

    }
}

