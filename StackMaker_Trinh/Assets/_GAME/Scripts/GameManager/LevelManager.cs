namespace _GAME.Scripts.Level
{
    using System;
    using System.Threading.Tasks;
    using _GAME.Scripts.BrickManager;
    using UnityEngine;

    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform    LevelRoot;
        [SerializeField] private Player       player;
        public                   int          totalLevelCount = 2;
        public static            LevelManager Instance              { get; set; }
        public                   GameObject   _currentLevelInstance { get; private set; }

        private int currentLevelIndex = 1;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public async Task LoadLevelAsync()
        {
            Debug.Log("Starting level loading...");
            if (BlockManager.Instance == null)
            {
                Debug.LogError("BlockManager.Instance is null during level loading!");
                return;
            }

            if (this._currentLevelInstance != null)
            {
                Destroy(this._currentLevelInstance);
            }

            var levelName = $"Level_{this.currentLevelIndex}";
            var request   = Resources.LoadAsync<GameObject>(levelName);

            while (!request.isDone)
            {
                await Task.Yield();
            }

            var prefab = request.asset as GameObject;
            if (prefab == null) return;

            this._currentLevelInstance = Instantiate(prefab, LevelRoot);
            BlockManager.Instance.SpawnBricks();
            BridgeManager.Instance.SortBridgeBlocks();
        }

        public async Task ReloadLevelAsync()
        {
            ResetLevelData();
            await LoadLevelAsync();
            PlacePlayer();
        }

        public async Task LoadNextLevelAsync()
        {
            if (!HasNextLevel()) return;

            this.currentLevelIndex++;
            await ReloadLevelAsync();
        }

        public async Task LoadSpecificLevelAsync(int savedLevel)
        {
            if (savedLevel < 1 || savedLevel > this.totalLevelCount)
            {
                savedLevel = 1;
            }

            this.currentLevelIndex = savedLevel;
            await ReloadLevelAsync();
        }

        private void ResetLevelData()
        {
            BlockManager.Instance.ClearBricks();
            BlockManager.Instance.groundList.Clear();
            BlockManager.Instance.TotalBrickCount = 0;
            BridgeManager.Instance.ClearAllBridgeBricks();
        }

        public bool HasNextLevel()
        {
            return currentLevelIndex + 1 <= totalLevelCount;
        }

        public int GetCurrentLevel()
        {
            return this.currentLevelIndex;
        }

        public void PlacePlayer()
        {
            var firstGroundBlock = BlockManager.Instance.FirstGroundBlock;
            if (firstGroundBlock == null) return;

            var groundHeight = firstGroundBlock.GetComponent<BoxCollider>().bounds.size.y;
            var spawnPos     = firstGroundBlock.transform.position + Vector3.up * groundHeight;
            this.player.transform.position = spawnPos;
        }
    }
}