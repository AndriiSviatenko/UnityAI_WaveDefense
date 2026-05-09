using UnityEngine;
using WaveDefense.Core;

namespace WaveDefense.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip attackClip;
        [SerializeField] private AudioClip deathClip;
        [SerializeField] private AudioClip hitClip;
        
        private AudioSource _source;

        private void Awake()
        {
            _source = gameObject.AddComponent<AudioSource>();
        }

        private void Start()
        {
            GameEvents.OnHeroAttack += () => PlaySound(attackClip);
            GameEvents.OnEnemyKilled += _ => PlaySound(deathClip);
            GameEvents.OnHeroDamage += _ => PlaySound(hitClip);
        }

        private void OnDestroy()
        {
            GameEvents.OnHeroAttack -= () => PlaySound(attackClip);
            GameEvents.OnEnemyKilled -= _ => PlaySound(deathClip);
            GameEvents.OnHeroDamage -= _ => PlaySound(hitClip);
        }

        private void PlaySound(AudioClip clip)
        {
            if (clip != null && _source != null)
                _source.PlayOneShot(clip);
        }
    }
}
