namespace _GAME.Scripts.Level
{
    using System;
    using _GAME.Scripts.BrickManager;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class LevelManager : MonoBehaviour
    {
        [SerializeField]                                               private Player       player;
        [FormerlySerializedAs("brickSpawnerManager")] [SerializeField] private BlockManager blockManager;
        private                                                                LevelManager Instance { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);

            InitLevel();

            PlacePlayer();
        }

        private void Start()
        {
        }

        public void InitLevel()
        {
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