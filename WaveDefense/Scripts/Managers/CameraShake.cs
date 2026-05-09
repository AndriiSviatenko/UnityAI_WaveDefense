using UnityEngine;
using WaveDefense.Core;
using System.Collections;

namespace WaveDefense.Managers
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private float duration = 0.1f;
        [SerializeField] private float magnitude = 0.2f;

        private Vector3 _originalPos;

        private void Start()
        {
            _originalPos = transform.localPosition;
            GameEvents.OnEnemyKilled += (score) => Shake();
            GameEvents.OnHeroDamage += (health) => Shake();
        }

        public void Shake()
        {
            StartCoroutine(ShakeRoutine());
        }

        private IEnumerator ShakeRoutine()
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                transform.localPosition = new Vector3(x, y, _originalPos.z);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = _originalPos;
        }
    }
}
