using Bullets;
using UnityEngine;

namespace BrackeysGameJam
{
    public static class Globals
    {
        public static GameManager gameManager => GameManager.Instance;
        public static BulletSystem bulletSystem => gameManager.GetComponent<BulletSystem>();
        public static GameObject player => gameManager.Player;
    }
}
