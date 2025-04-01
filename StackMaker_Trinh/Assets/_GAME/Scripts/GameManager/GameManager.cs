namespace _GAME.Scripts.GameManager
{
    using System;
    using _GAME.Scripts.Camera;
    using _GAME.Scripts.GameManager.Audio;
    using _GAME.Scripts.Level;
    using _GAME.Scripts.UI;
    using UnityEngine;

    public enum GameState
    {
        None,
        Playing,
        Win,
        Lose,
        Pause
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private AudioEventChannelSO sfxChannel;
        public static GameManager Instance { get; private set; }

        public GameState CurrentGameState { get; private set; } = GameState.None;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void Init()
        {
        }

        public void SetGameState(GameState newState)
        {
            if(CurrentGameState == newState) return;

            CurrentGameState = newState;

            switch (newState)
            {
                case GameState.Win:
                    GameEvent.OnPlayerWin?.Invoke();
                    UIManager.Instance.ShowWinPanel();
                    sfxChannel.Raise(SFXType.Win);

                    this.SaveCurrentLevel();

                    Invoke(nameof(LoadNextLevel), 2f);
                    break;
                case GameState.Playing:
                    HandlePlaying();
                    break;
                case GameState.Lose:
                    break;
                case GameState.Pause:
                    HandlePause();
                    break;
            }
        }

        private void LoadNextLevel()
        {
            SetGameState(GameState.Playing);
            LevelManager.Instance.LoadNextLevelAsync();
        }

        private void SaveCurrentLevel()
        {
            PlayerPrefs.SetInt("SavedLevel", LevelManager.Instance.GetCurrentLevel() + 1);
            PlayerPrefs.Save();
        }

        private void HandlePause()
        {
            UIManager.Instance.ShowPausePanel();
            Time.timeScale = 0;
            GameEvent.OnInputPermissionChanged?.Invoke(false);
            CameraController.Instance.EnableCamera(false);
        }

        private void HandlePlaying()
        {
            UIManager.Instance.HideWinPanel();
            UIManager.Instance.ShowIngameUI();

            Time.timeScale = 1;
            GameEvent.OnInputPermissionChanged?.Invoke(true);
            CameraController.Instance.EnableCamera(true);
        }

        private void Update()
        {
            Debug.Log(CurrentGameState);
        }
    }
}