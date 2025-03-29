namespace _GAME.Scripts.UI
{
    using UnityEngine;

    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        [SerializeField] private GameObject Canvas;
        [SerializeField] private GameObject winPanel;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void Init()
        {
            this.HideWinPanel();
            this.ShowCanvas();
        }

        private void ShowCanvas()
        {
            this.Canvas?.SetActive(true);
        }

        public void ShowWinPanel()
        {
            this.winPanel?.SetActive(true);
        }

        public void HideWinPanel()
        {
            this.winPanel?.SetActive(false);
        }
    }
}