using System;
using UnityEngine;

namespace LiquidForce
{
    public class Application : MonoBehaviour
    {
        public static Application Instance;
        public CameraFader cameraFader;

        private void Awake()
        {
            Instance = this;
        }
    }
}
