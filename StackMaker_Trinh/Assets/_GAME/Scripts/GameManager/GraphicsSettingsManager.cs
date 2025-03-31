namespace _GAME.Scripts.GameManager
{
    using System;
    using UnityEngine;

    public enum GraphicsQuality
    {
        Low,
        Medium,
        High
    }
    public class GraphicsSettingsManager : MonoBehaviour
    {
        public static GraphicsSettingsManager Instance { get; private set; }
        public GraphicsQuality CurrentGraphicsQuality { get; private set; } = GraphicsQuality.Medium;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void ApplyGraphicsSettings(GraphicsQuality quality)
        {
            CurrentGraphicsQuality = quality;
            switch (quality)
            {
                case GraphicsQuality.Low:
                    Application.targetFrameRate = 30;
                    QualitySettings.SetQualityLevel(0);
                    break;
                case GraphicsQuality.Medium:
                    Application.targetFrameRate = 60;
                    QualitySettings.SetQualityLevel(2);
                    break;
                case GraphicsQuality.High:
                    Application.targetFrameRate = 60;
                    QualitySettings.SetQualityLevel(4);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(quality), quality, null);
            }

            Debug.Log(quality);
        }
    }
}