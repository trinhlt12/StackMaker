namespace _GAME.Scripts.Level
{
    using System;
    using _GAME.Scripts.BrickManager;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform    LevelRoot;
        [SerializeField] private Player       player;
        public                   int          totalLevelCount = 2;
        public static            LevelManager Instance { get; set; }
        public GameObject _currentLevelInstance { get; private set; }

        private int currentLevelIndex = 1;

        private int indexToPlacePlayer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void LoadLevel()
        {
            BlockManager.Instance.groundList.Clear();
            if (this._currentLevelInstance != null)
            {
                Destroy(this._currentLevelInstance);
            }

            string levelName = $"Level_{this.currentLevelIndex}";
            var    prefab    = Resources.Load<GameObject>(levelName);

            if (prefab == null) return;
            this._currentLevelInstance = Instantiate(prefab, LevelRoot);
        }

        public void LoadNextLevel()
        {
            if (!HasNextLevel())
            {
                return;
            }
            this.currentLevelIndex++;
            ResetLevelData();

            Init();
            this.PlacePlayer();
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

        public void Init()
        {
            LoadLevel();
            this.player        = FindObjectOfType<Player>();

            BlockManager.Instance.Init();
            BridgeManager.Instance.Init();

            BridgeManager.Instance.SortBridgeBlocks();
        }

        public void PlacePlayer()
        {
            var firstGroundBlock = BlockManager.Instance.FirstGroundBlock;
            if (firstGroundBlock == null)
            {
                return;
            }

            var groundHeight = firstGroundBlock.GetComponent<BoxCollider>().bounds.size.y;
            var spawnPos     = firstGroundBlock.transform.position + Vector3.up * groundHeight;

            this.player.transform.position = spawnPos;
        }

    }
}