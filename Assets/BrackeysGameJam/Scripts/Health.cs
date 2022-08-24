using System;
using UnityEngine;
using UnityEngine.Events;

namespace BrackeysGameJam
{
    public class Health : MonoBehaviour
    {
        public float maxHP = 100.0f;
        public UnityEvent died;
        public UnityEvent healthChanged;
        public UnityEvent damaged;
        public UnityEvent healed;
        
        private float _currentHP;
        
        public float CurrentHP => _currentHP;
        public bool IsDead => _currentHP <= 0;

        private void Start()
        {
            _currentHP = maxHP;
            healthChanged.Invoke();
        }

        public void Damage(float amount)
        {
            if (IsDead) return;
            
            _currentHP = Mathf.Clamp(_currentHP - amount, 0, maxHP);
            healthChanged.Invoke();
            damaged.Invoke();
            
            if (IsDead) died.Invoke();
        }

        public void Heal(float amount)
        {
            _currentHP = Mathf.Clamp(_currentHP + amount, 0, maxHP);
            healthChanged.Invoke();
            healed.Invoke();
        }
    }
}