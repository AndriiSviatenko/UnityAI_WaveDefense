using UnityEngine;

namespace WaveDefense.UI
{
    public class VFX_SimpleSlash : MonoBehaviour
    {
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private Vector3 scaleTarget = new Vector3(1.5f, 1.5f, 1f);
        
        private SpriteRenderer _sr;
        private float _elapsed;
        private Vector3 _startScale;
        private Color _startColor;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _startScale = transform.localScale;
            if (_sr != null) _startColor = _sr.color;
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            float t = _elapsed / duration;

            transform.localScale = Vector3.Lerp(_startScale, Vector3.Scale(_startScale, scaleTarget), t);
            
            if (_sr != null)
            {
                Color c = _startColor;
                c.a = Mathf.Lerp(_startColor.a, 0, t);
                _sr.color = c;
            }

            if (t >= 1f) Destroy(gameObject);
        }
    }
}
