namespace _GAME.Scripts.UI
{
    using UnityEngine;

    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
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
        }

        public void ShowWinPanel()
        {
            Debug.Log("WIN PANEL");
            this.winPanel?.SetActive(true);
        }

        public void HideWinPanel()
        {
            this.winPanel?.SetActive(false);
        }
    }
}