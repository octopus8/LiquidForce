using System;
using System.Collections.Generic;
using UnityEngine;

namespace LiquidForce
{
    public class ObjectFollower : MonoBehaviour
    {
        public GameObject source { private get; set; }
        
        private List<GameObject> targets;
        
        public Moment moment { private get; set; } = Moment.OnUpdate;

        private void Awake()
        {
            targets = new List<GameObject>();
        }

        public void AddTarget(GameObject target)
        {
            targets.Add(target);
        }

        public void ClearTargets()
        {
            targets.Clear();
        }

        private void FixedUpdate()
        {
            if (moment == Moment.OnFixedUpdate)
            {
                UpdateTransform();    
            }
            
        }

        private void Update()
        {
            if (moment == Moment.OnUpdate)
            {
                UpdateTransform();    
            }
        }

        private void LateUpdate()
        {
            if (moment == Moment.OnLateUpdate)
            {
                UpdateTransform();    
            }
        }

        private void OnPreRender()
        {
            if (moment == Moment.OnPreRender)
            {
                UpdateTransform();    
            }
        }

        private void OnPreCull()
        {
            if (moment == Moment.OnPreCull)
            {
                UpdateTransform();    
            }
            
        }

        private void UpdateTransform()
        {
            if (null == source)
            {
                source = gameObject;
            }
            
            foreach (var target in targets)
            {
                target.transform.SetPositionAndRotation(source.transform.position, source.transform.rotation);
                target.transform.localScale = source.transform.localScale;
            }
        }
        
        
        
        
        
    
        public enum Moment
        {
            OnFixedUpdate,
            OnUpdate,
            OnLateUpdate,
            OnPreRender,
            OnPreCull
        }
    
    }
    
}
