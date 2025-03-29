namespace _GAME.Scripts.Level
{
    using System;
    using _GAME.Scripts.BrickManager;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class LevelManager : MonoBehaviour
    {
        [SerializeField]                                               private Transform     LevelRoot;
        [SerializeField]                                               private Player        player;
        [FormerlySerializedAs("brickSpawnerManager")] [SerializeField] private BlockManager  blockManager;
        public int totalLevelCount = 2;
        private                                                                BridgeManager bridgeManager;
        public static                                                          LevelManager  Instance { get; set; }
        private                                                                GameObject    _currentLevelInstance;

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

        private void LoadLevel()
        {
            if (this._currentLevelInstance != null)
            {
                Destroy(this._currentLevelInstance);
            }

            string levelName = $"Level_{this.currentLevelIndex}";
            var    prefab    = Resources.Load<GameObject>(levelName);

            if(prefab == null) return;
            this._currentLevelInstance = Instantiate(prefab, LevelRoot);

        }

        public void LoadNextLevel()
        {
            if (!HasNextLevel())
            {
                return;
            }
            this.currentLevelIndex++;
            Init();
            BridgeManager.Instance.ClearAllBridgeBricks();
            this.PlacePlayer();
        }

        public bool HasNextLevel()
        {
            return currentLevelIndex + 1 <= totalLevelCount;
        }


        public void Init()
        {
            LoadLevel();
            this.player              = FindObjectOfType<Player>();
            this.bridgeManager = FindObjectOfType<BridgeManager>();
            this.blockManager = FindObjectOfType<BlockManager>();

            this.blockManager.Init();
            this.bridgeManager.Init();

            BridgeManager.Instance.SortBridgeBlocks();
        }


        public void PlacePlayer()
        {
            Debug.Log(this.blockManager.FirstGroundPosition);

            var firstGroundBlockPosition = this.blockManager.FirstGroundPosition;
            var groundHeight = this.blockManager.FirstGroundBlock.GetComponent<BoxCollider>().bounds.size.y;

            this.player.transform.position = firstGroundBlockPosition + Vector3.up * this.player.GetComponent<CapsuleCollider>().height/2;
        }
    }
}