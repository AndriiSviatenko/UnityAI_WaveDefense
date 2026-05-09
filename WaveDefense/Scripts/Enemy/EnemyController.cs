using UnityEngine;
using WaveDefense.Core;
using WaveDefense.Hero;

namespace WaveDefense.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyData data;
        private float _currentHealth;
        private bool _isDead;

        private void OnEnable()
        {
            if (data != null)
                _currentHealth = data.health;
            _isDead = false;
        }

        private void Update()
        {
            if (_isDead || data == null) return;

            // Move towards hero (center)
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, data.speed * Time.deltaTime);

            // Flip sprite based on direction
            if (transform.position.x > 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

            // Check if reached hero
            if (Mathf.Abs(transform.position.x) < 0.8f)
            {
                var hero = FindFirstObjectByType<Hero.HeroController>();
                if (hero != null) hero.TakeDamage(data.damage);
                Die(false); // Die without scoring
            }
        }

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            if (_currentHealth <= 0 && !_isDead) Die(true);
        }

        private void Die(bool scored)
        {
            _isDead = true;
            if (scored && data != null)
                GameEvents.OnEnemyKilled?.Invoke(data.killValue);
            
            if (Managers.PoolManager.Instance != null && data != null && data.prefab != null)
                Managers.PoolManager.Instance.ReturnToPool(data.prefab, gameObject);
            else
                gameObject.SetActive(false);
        }
}
}
