namespace _GAME.Scripts.GameManager
{
    using System;
    using System.Collections;
    using _GAME.Scripts.BrickManager;
    using _GAME.Scripts.UI;
    using UnityEngine;

    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Level.LevelManager levelManager;
        [SerializeField] private InputManager inputManager;

        private void Awake()
        {
            OnGameStart();
        }

        private void Start()
        {
            GraphicsSettingsManager.Instance.ApplyGraphicsSettings(GraphicsQuality.Low);

            this.levelManager.PlacePlayer();
        }

        private void OnGameStart()
        {
            GameEvent.OnInputPermissionChanged?.Invoke(false);
            StartCoroutine(PreloadGame());
        }

        private IEnumerator PreloadGame()
        {
            this.uiManager.ShowMainMenuPanel();
            this.uiManager.HidePlayButton();
            uiManager.ShowLoadingPanel();

            var progress = 0f;
            uiManager.UpdateLoadingBar(progress);

            // Initialize GameManager
            gameManager.Init();
            progress += 0.33f;
            uiManager.UpdateLoadingBar(progress);

            // Initialize LevelManager
            levelManager.Init();
            progress += 0.33f;
            uiManager.UpdateLoadingBar(progress);

            // Initialize InputManager
            inputManager.Init();
            progress = 1f;
            uiManager.UpdateLoadingBar(progress);

            yield return new WaitForSeconds(0.1f);

            uiManager.HideLoadingPanel();
            this.uiManager.ShowPlayButton();

            GraphicsSettingsManager.Instance.ApplyGraphicsSettings(GraphicsQuality.Medium);
            yield break;
        }
    }
}