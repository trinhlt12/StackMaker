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

        private bool _isInitialized = false;

        private void Awake()
        {
            GraphicsSettingsManager.Instance.ApplyGraphicsSettings(GraphicsQuality.Low);

            OnGameStart();
        }

        private void Start()
        {

            this.levelManager.PlacePlayer();
        }

        private void OnGameStart()
        {
            GameEvent.OnInputPermissionChanged?.Invoke(false);
            StartCoroutine(this.PreloadGameRoutine());
        }

        private IEnumerator PreloadGameRoutine()
        {
            // Setup UI for loading
            this.uiManager.ShowMainMenuPanel();
            this.uiManager.HidePlayButton();
            uiManager.ShowLoadingPanel();

            var progress = 0f;
            uiManager.UpdateLoadingBar(progress);

            // Initialize managers in the correct order
            Debug.Log("Initializing GameManager...");
            gameManager.Init();
            progress += 0.25f;
            uiManager.UpdateLoadingBar(progress);
            yield return null; // Allow a frame to process

            Debug.Log("Initializing BlockManager...");
            if (BlockManager.Instance == null)
            {
                Debug.LogError("BlockManager.Instance is null! Check script execution order.");
                yield break;
            }

            // Initialize BlockManager first without spawning bricks
            BlockManager.Instance.InitializePool();
            progress += 0.25f;
            uiManager.UpdateLoadingBar(progress);
            yield return null; // Allow a frame to process

            Debug.Log("Initializing LevelManager...");
            // Start level loading
            var levelLoadTask = levelManager.LoadLevelAsync();

            // Wait until level is fully loaded
            while (!levelLoadTask.IsCompleted)
            {
                // Could update progress bar based on loading status here
                yield return null;
            }

            // Now that level is loaded, initialize remaining systems
            progress += 0.25f;
            uiManager.UpdateLoadingBar(progress);

            Debug.Log("Initializing InputManager...");
            inputManager.Init();
            progress = 1f;
            uiManager.UpdateLoadingBar(progress);

            yield return new WaitForSeconds(0.1f);

            // Hide loading UI
            uiManager.HideLoadingPanel();
            this.uiManager.ShowPlayButton();

            // Now that everything is loaded, we can place the player and upgrade graphics
            levelManager.PlacePlayer();
            GraphicsSettingsManager.Instance.ApplyGraphicsSettings(GraphicsQuality.Medium);

            _isInitialized = true;
            Debug.Log("Game initialization complete!");
        }
    }
}