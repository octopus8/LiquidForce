using System;
using UnityEngine;

namespace LiquidForce
{
    public class DeviceTracking : MonoBehaviour
    {
        [SerializeField]
        private GameObject head;
        
        
        private ObjectFollower headObjectFollower;


        private void Awake()
        {
            headObjectFollower = gameObject.AddComponent<ObjectFollower>();
            headObjectFollower.moment = ObjectFollower.Moment.OnFixedUpdate;

            if (null != head)
            {
                headObjectFollower.source = head;
            }
            else
            {
                Debug.LogError($"Head object not specified.");
            }
        }

        public void AddHeadTarget(GameObject target)
        {
            headObjectFollower.AddTarget(target);
        }
        
    }
}
