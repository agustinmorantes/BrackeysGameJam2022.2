using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Bullets
{
    public class BulletSystem : MonoBehaviour
    {
        public static BulletSystem instance { get; private set; }

        private List<BulletInstance> bullets = new List<BulletInstance>();

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Multiple instances detected!", instance);
                DestroyImmediate(this);
                return;
            }

            instance = this;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        private void Update()
        {
            var toRemove = new List<BulletInstance>();
            
            foreach (var b in bullets)
            {
                if (b.GameObject == null)
                {
                    toRemove.Add(b);
                    continue;
                }

                var t = b.Transform;
                b.Transform.Translate(t.forward * (b.Properties.speed * Time.deltaTime));
            }

            bullets = bullets.Except(toRemove).ToList();
        }

        public void Shoot(Vector3 pos, Vector3 dir, BulletProperties properties)
        {
            var go = Instantiate(properties.prefab, pos, Quaternion.LookRotation(dir, Vector3.up));
            var rb = go.GetComponent<Rigidbody>();
            var bInstance = new BulletInstance()
                { Properties = properties, GameObject = go, Transform = go.transform, Rigidbody = rb };
            bullets.Add(bInstance);
        }

        private struct BulletInstance
        {
            public BulletProperties Properties { get; set; }
            public GameObject GameObject { get; set; }
            public Transform Transform { get; set; }
            public Rigidbody Rigidbody { get; set; }
        }
    }
}