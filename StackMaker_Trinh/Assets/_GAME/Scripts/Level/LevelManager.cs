namespace _GAME.Scripts.Level
{
    using System;
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
        }

        private void Start()
        {
            this.InitLevel();
        }

        public void InitLevel()
        {
        }
    }
}