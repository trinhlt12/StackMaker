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
            GraphicsSettingsManager.Instance.ApplyGraphicsSettings(GraphicsQuality.Medium);

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

            yield return new WaitForSeconds(0.2f);
            progress += 0.1f;
            uiManager.UpdateLoadingBar(progress);

            gameManager.Init();
            yield return new WaitForSeconds(0.2f);
            progress += 0.4f;
            uiManager.UpdateLoadingBar(progress);

            levelManager.Init();
            yield return new WaitForSeconds(0.2f);
            progress += 0.8f;
            uiManager.UpdateLoadingBar(progress);

            inputManager.Init();
            yield return new WaitForSeconds(0.1f);
            progress = 1f;
            uiManager.UpdateLoadingBar(progress);

            yield return new WaitForSeconds(0.3f);

            uiManager.HideLoadingPanel();
            uiManager.ShowMainMenuPanel();
            this.uiManager.ShowPlayButton();
            this.uiManager.ShowPlayButton();

        }
    }
}