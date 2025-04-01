namespace _GAME.Scripts.UI
{
    using _GAME.Scripts.GameManager;
    using _GAME.Scripts.Level;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        [SerializeField] private GameObject Canvas;
        [SerializeField] private GameObject winPanel;
        [Header("Loading UI")]
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private Image loadingBarFill;

        [Header("Main Menu")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject playButton;

        [Header("Settings")]
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject     pauseButton;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            Init();
        }

        private void Init()
        {
            this.HideWinPanel();
            this.ShowCanvas();
            this.HidePausePanel();
        }

        public void ShowIngameUI()
        {
            this.pauseButton.SetActive(true);
        }

        private void HideIngameUI()
        {
            this.pauseButton.SetActive(false);
        }

        public void ShowLoadingPanel()
        {
            this.loadingPanel?.SetActive(true);
            loadingBarFill.fillAmount = 0;
        }

        public void HideLoadingPanel()
        {
            this.loadingPanel?.SetActive(false);
        }

        public void ShowMainMenuPanel()
        {
            this.mainMenuPanel?.SetActive(true);
        }

        public void HideMainMenuPanel()
        {
            this.mainMenuPanel?.SetActive(false);
        }

        public void UpdateLoadingBar(float progress)
        {
            loadingBarFill.fillAmount = Mathf.Clamp01(progress);
        }

        private void ShowCanvas()
        {
            this.Canvas?.SetActive(true);
        }

        public void ShowWinPanel()
        {
            this.winPanel?.SetActive(true);
        }

        public void HideWinPanel()
        {
            this.winPanel?.SetActive(false);
        }

        public void ShowPlayButton()
        {
            this.playButton?.SetActive(true);
        }

        public void HidePlayButton()
        {
            this.playButton?.SetActive(false);
        }

        public void OnClickPlayButton()
        {
            HideMainMenuPanel();
            var savedLevel = PlayerPrefs.GetInt("SavedLevel", 1);
            LevelManager.Instance.LoadSpecificLevelAsync(savedLevel);

            GameManager.Instance.SetGameState(GameState.Playing);
        }

        public void OnClickPauseButton()
        {
            GameManager.Instance.SetGameState(GameState.Pause);
            this.ShowPausePanel();
        }

        public void OnClickClosePausePanelButton()
        {
            HidePausePanel();
        }

        public void OnClickRestartButton()
        {
            LevelManager.Instance.ReloadLevelAsync();
            this.HidePausePanel();
        }

        public void ShowPausePanel()
        {
            this.pausePanel?.SetActive(true);
        }

        public void HidePausePanel()
        {
            this.pausePanel?.SetActive(false);
            GameManager.Instance.SetGameState(GameState.Playing);
        }
    }
}