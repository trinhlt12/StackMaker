namespace _GAME.Scripts.GameManager
{
    using System.Collections;
    using _GAME.Scripts.BrickManager;
    using _GAME.Scripts.Level;
    using _GAME.Scripts.UI;
    using DG.Tweening;
    using UnityEngine;

    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private InputManager inputManager;

        private bool _isInitialized = false;

        private void Awake()
        {
            OnGameStart();
        }

        private void OnGameStart()
        {
            /*GraphicsSettingsManager.Instance.ApplyGraphicsSettings(GraphicsQuality.Low);*/
            DOTween.useSafeMode = true;
            GameEvent.OnInputPermissionChanged?.Invoke(false);
            StartCoroutine(PreloadGameRoutine());
        }

        private IEnumerator PreloadGameRoutine()
        {
            uiManager.ShowMainMenuPanel();
            uiManager.HidePlayButton();
            uiManager.ShowLoadingPanel();

            float progress = 0f;
            uiManager.UpdateLoadingBar(progress);

            gameManager.Init();
            progress += 0.25f;
            uiManager.UpdateLoadingBar(progress);
            yield return null;

            if (BlockManager.Instance == null)
            {
                Debug.LogError("BlockManager.Instance is null! Check script execution order.");
                yield break;
            }

            BlockManager.Instance.InitializePool();
            progress += 0.25f;
            uiManager.UpdateLoadingBar(progress);
            yield return null;

            // Start async task and wait until done using coroutine-compatible pattern
            bool done = false;
            System.Threading.Tasks.Task loadTask = levelManager.ReloadLevelAsync();
            loadTask.ContinueWith(_ => done = true);
            while (!done)
            {
                yield return null;
            }

            progress += 0.25f;
            uiManager.UpdateLoadingBar(progress);

            inputManager.Init();
            progress = 1f;
            uiManager.UpdateLoadingBar(progress);

            yield return new WaitForSeconds(0.1f);

            uiManager.HideLoadingPanel();
            uiManager.ShowPlayButton();
            GraphicsSettingsManager.Instance.ApplyGraphicsSettings(GraphicsQuality.Medium);

            _isInitialized = true;
            Debug.Log("Game initialization complete!");
        }
    }
}