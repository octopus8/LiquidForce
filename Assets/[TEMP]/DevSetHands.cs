using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Hands.Samples.VisualizerSample;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace LiquidForce
{



    public class DevSetHands : MonoBehaviour
    {
        public XRInputModalityManager xrInputModalityManager;

        public HandVisualizer handVisualizer;

        public GameObject leftHandPrefab;
        public GameObject rightHandPrefab;

        public GameObject handParent;

        
        

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        IEnumerator Start()
        {
            yield return new WaitForSeconds(10);

            xrInputModalityManager.leftHand = Instantiate(leftHandPrefab, handParent.transform);
            xrInputModalityManager.rightHand = Instantiate(rightHandPrefab, handParent.transform);

            handVisualizer.LeftHandInteractionVisual = xrInputModalityManager.leftHand.GetComponent<HandComponents>().InteractionVisual;
            handVisualizer.RightHandInteractionVisual = xrInputModalityManager.rightHand.GetComponent<HandComponents>().InteractionVisual;

            Debug.Log("Delayed Start Complete.");
            
        }



        // Update is called once per frame
        void Update()
        {

        }
    }
}
