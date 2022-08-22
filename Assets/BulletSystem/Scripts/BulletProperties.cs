using UnityEngine;

namespace Bullets
{
    [CreateAssetMenu(menuName = "Bullets/Bullet Properties", fileName = "New Bullet Type")]
    public class BulletProperties : ScriptableObject
    {
        public float speed;
        public int damage;
        public BulletOrigin origin;
        public GameObject prefab;

        public enum BulletOrigin
        {
            Player,
            Enemy
        }
    }
}