 namespace _GAME.Scripts.GameManager.Audio
{
    using System;
    using UnityEngine;
    public enum SFXType
    {
        Pickup,
        Place,
        Win,
    }


    [CreateAssetMenu(menuName = "Audio/Audio Event Channel")]
    public class AudioEventChannelSO : ScriptableObject
    {

        public Action<SFXType> OnPlaySFX;

        public void Raise(SFXType sfxType)
        {
            this.OnPlaySFX?.Invoke(sfxType);
        }
    }
}