namespace _GAME.Scripts.GameManager
{
    using System;
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        public GameManager Instance { get; private set; }

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

    }
}