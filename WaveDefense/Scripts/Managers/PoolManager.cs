using System.Collections.Generic;
using UnityEngine;

namespace WaveDefense.Managers
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        private Dictionary<GameObject, Queue<GameObject>> _pools = new Dictionary<GameObject, Queue<GameObject>>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (!_pools.ContainsKey(prefab))
                _pools.Add(prefab, new Queue<GameObject>());

            GameObject obj;
            if (_pools[prefab].Count > 0)
            {
                obj = _pools[prefab].Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
            }
            else
            {
                obj = Instantiate(prefab, position, rotation);
            }

            return obj;
        }

        public void ReturnToPool(GameObject prefab, GameObject obj)
        {
            obj.SetActive(false);
            if (!_pools.ContainsKey(prefab))
                _pools.Add(prefab, new Queue<GameObject>());
            
            _pools[prefab].Enqueue(obj);
        }
    }
}
