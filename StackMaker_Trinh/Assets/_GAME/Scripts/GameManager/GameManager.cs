namespace _GAME.Scripts.GameManager
{
    using System;
    using _GAME.Scripts.UI;
    using UnityEngine;

    public enum GameState
    {
        Playing,
        Win,
        Lose
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameState CurrentGameState { get; private set; } = GameState.Playing;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public void SetGameState(GameState newState)
        {
            if(CurrentGameState == newState) return;

            CurrentGameState = newState;

            switch (newState)
            {
                case GameState.Win:
                    Debug.Log("WIN");
                    GameEvent.OnPlayerWin?.Invoke();
                    UIManager.Instance.ShowWinPanel();
                    break;
                case GameState.Lose:
                    break;
            }
        }

    }
}