using System;
using UnityEngine;

namespace LiquidForce
{
    /// <summary>
    /// A required component of the Application, this component provides functionality for object to follow tracked devices.
    /// </summary>
    public class DeviceTracking : MonoBehaviour
    {
        /// <summary>
        /// The source head GameObject.
        /// </summary>
        [SerializeField]
        private GameObject head;
        
        /// <summary>
        /// The head object follower, added to the GameObject by `Awake`.
        /// </summary>
        private ObjectFollower headObjectFollower;


        private void Awake()
        {
            // Add and initialize the head object follower component.
            headObjectFollower = gameObject.AddComponent<ObjectFollower>();
            headObjectFollower.moment = ObjectFollower.Moment.OnFixedUpdate;

            // Set the head object follower source.
            if (null == head)
            {
                Debug.LogError($"Head object not specified.");
                return;
            }
            headObjectFollower.source = head;
        }

        
        /// <summary>
        /// Adds an object to follow the head.
        /// </summary>
        /// <param name="target">The object to follow the head.</param>
        public void AddHeadFollower(GameObject target)
        {
            headObjectFollower.AddTarget(target);
        }
    }
}
