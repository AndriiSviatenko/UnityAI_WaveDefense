using UnityEngine;
using UnityEngine.InputSystem;
using WaveDefense.Core;

namespace WaveDefense.Hero
{
    public class HeroController : MonoBehaviour
    {
        [Header("Combat Settings")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float attackRange = 2.5f;
        [SerializeField] private LayerMask enemyLayer;

        [Header("Feedback")]
        [SerializeField] private ParticleSystem attackVFX;
        [SerializeField] private GameObject attackEffectPrefab;
        [SerializeField] private GameObject leftZoneVisual;
        [SerializeField] private GameObject rightZoneVisual;
        
        private float _currentHealth;
        private Animator _animator;
        
        // Note: WaveDefenseActions will be generated later
        private InputAction _attackLeft;
        private InputAction _attackRight;

        private void Awake()
        {
            _currentHealth = maxHealth;
            _animator = GetComponent<Animator>();
            
            if (leftZoneVisual != null) leftZoneVisual.SetActive(true);
            if (rightZoneVisual != null) rightZoneVisual.SetActive(true);
        }

        private void Start()
        {
            // Fallback for when we haven't generated the class yet or are using project-wide actions
            var actions = InputSystem.actions;
            if (actions != null)
            {
                _attackLeft = actions.FindAction("AttackLeft");
                _attackRight = actions.FindAction("AttackRight");
                
                if (_attackLeft != null) _attackLeft.performed += _ => Attack(Vector2.left);
                if (_attackRight != null) _attackRight.performed += _ => Attack(Vector2.right);
            }
        }

        private void Attack(Vector2 direction)
        {
            if (_currentHealth <= 0) return;

            GameEvents.OnHeroAttack?.Invoke();

            if (_animator != null)
                _animator.SetTrigger(direction == Vector2.left ? "AttackLeft" : "AttackRight");
            
            if (attackVFX != null) attackVFX.Play();

            // Spawn visual slash effect
            if (attackEffectPrefab != null)
            {
                Vector3 spawnPos = transform.position + (Vector3)direction * (attackRange * 0.5f);
                GameObject effect = Instantiate(attackEffectPrefab, spawnPos, Quaternion.identity);
                effect.transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
                Destroy(effect, 0.5f); // Auto destroy
            }

            Vector2 attackPos = (Vector2)transform.position + direction * (attackRange * 0.5f);
            Collider2D[] hits = Physics2D.OverlapBoxAll(attackPos, new Vector2(attackRange, 1.5f), 0, enemyLayer);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<Enemy.EnemyController>(out var enemy))
                    enemy.TakeDamage(100); 
            }
        }

        public void TakeDamage(float amount)
        {
            if (_currentHealth <= 0) return;

            _currentHealth -= amount;
            GameEvents.OnHeroDamage?.Invoke(_currentHealth / maxHealth);

            if (_currentHealth <= 0)
            {
                if (_animator != null) _animator.SetTrigger("Die");
                GameEvents.OnGameOver?.Invoke();
                GameEvents.OnHeroDeath?.Invoke();
            }
        }
    }
}
