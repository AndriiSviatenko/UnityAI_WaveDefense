using System;

namespace WaveDefense.Core
{
    public static class GameEvents
    {
        // Enemy Events
        public static Action<int> OnEnemyKilled; // score
        
        // Hero Events
        public static Action OnHeroAttack;
        public static Action<float> OnHeroDamage; // health percentage
public static Action OnHeroDeath;
        
        // Game State Events
        public static Action OnGameOver;
        public static Action OnGameRestart;
    }
}
