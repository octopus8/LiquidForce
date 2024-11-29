using System;
using UnityEngine;


namespace LiquidForce
{
    public class Startup : MonoBehaviour
    {
        private void Start()
        {
            TestClass.Instance.cameraFader.FadeCameraIn();
        }
    }
}
