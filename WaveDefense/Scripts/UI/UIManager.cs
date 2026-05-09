using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WaveDefense.Core;
using UnityEngine.SceneManagement;

namespace WaveDefense.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("HUD")]
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Slider healthSlider;

        [Header("Game Over")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TMP_Text finalScoreText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button quitButton;

        private int _currentScore;

        private void Start()
        {
            _currentScore = 0;
            UpdateScoreUI();
            
            if (gameOverPanel != null) gameOverPanel.SetActive(false);

            GameEvents.OnEnemyKilled += AddScore;
            GameEvents.OnHeroDamage += UpdateHealthUI;
            GameEvents.OnGameOver += ShowGameOver;

            if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
            if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
        }

        private void OnDestroy()
        {
            GameEvents.OnEnemyKilled -= AddScore;
            GameEvents.OnHeroDamage -= UpdateHealthUI;
            GameEvents.OnGameOver -= ShowGameOver;
        }

        private void AddScore(int amount)
        {
            _currentScore += amount;
            UpdateScoreUI();
        }

        private void UpdateScoreUI()
        {
            if (scoreText != null) scoreText.text = $"Score: {_currentScore}";
        }

        private void UpdateHealthUI(float percentage)
        {
            if (healthSlider != null) healthSlider.value = percentage;
        }

        private void ShowGameOver()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                if (finalScoreText != null) finalScoreText.text = $"Final Score: {_currentScore}";
            }
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
