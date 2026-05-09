using UnityEngine;

namespace WaveDefense.Core
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "WaveDefense/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public string enemyName;
        public float health = 10f;
        public float speed = 2f;
        public float damage = 10f;
        public int killValue = 100;
        public GameObject prefab;
    }
}
