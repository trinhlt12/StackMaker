namespace _GAME.Scripts.GameManager
{
    using System;
    using _GAME.Scripts.BrickManager;
    using _GAME.Scripts.UI;
    using UnityEngine;

    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Level.LevelManager levelManager;
        [SerializeField] private InputManager inputManager;

        private void Start()
        {
            uiManager.Init();
            gameManager.Init();
            levelManager.Init();
            inputManager.Init();

            this.levelManager.PlacePlayer();
        }
    }
}