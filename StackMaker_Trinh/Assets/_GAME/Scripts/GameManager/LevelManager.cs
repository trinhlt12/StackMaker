namespace _GAME.Scripts.Level
{
    using System;
    using _GAME.Scripts.BrickManager;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform   LevelRoot;
        [SerializeField]                                               private Player        player;
        [FormerlySerializedAs("brickSpawnerManager")] [SerializeField] private BlockManager  blockManager;
        private                                                                BridgeManager bridgeManager;
        private                                                                LevelManager  Instance { get; set; }
        private GameObject _currentLevelInstance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void LoadLevel(string levelName)
        {
            if (this._currentLevelInstance != null)
            {
                Destroy(this._currentLevelInstance);
            }

            var prefab = Resources.Load<GameObject>($"{levelName}");

            if (prefab != null)
            {
                this._currentLevelInstance = Instantiate(prefab, LevelRoot);
            }
            else
            {
                Debug.LogError($"{levelName} not found");
            }
        }

        public void Init()
        {
            LoadLevel("Level_1");
            this.player              = FindObjectOfType<Player>();
            this.bridgeManager = FindObjectOfType<BridgeManager>();

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