using System;
using UnityEngine;

namespace BrackeysGameJam
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance) return _instance;

                var existing = FindObjectOfType<GameManager>();
                if (existing) _instance = existing;
                else
                {
                    var prefab = Resources.Load<GameObject>("Prefabs/GameManager");
                    var go = Instantiate(prefab);
                    _instance = go.GetComponent<GameManager>();
                }
                
                DontDestroyOnLoad(_instance.gameObject);
                return _instance;
            }
        }

        private GameObject player;
        public GameObject Player
        {
            get
            {
                if (player) return player;

                player = GameObject.FindWithTag("Player");
                if (!player) throw new Exception("No player in scene!");
            
                return player;
            }
        }
        
        private void Awake()
        {
            if (Instance == this) return;
            
            Debug.LogError("Game manager must not be in scene!");
            DestroyImmediate(gameObject);
        }
    }
}
