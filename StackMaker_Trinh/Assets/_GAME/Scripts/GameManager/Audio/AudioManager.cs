using UnityEngine;
using UnityEngine.UI;namespace _GAME.Scripts.GameManager.Audio
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioImage muteImage;

        [Header("Sound Mappping")]
        [SerializeField] private List<SFXType> types;
        [SerializeField] private List<AudioCue> clips;

        private Dictionary<SFXType, AudioCue> clipDict = new Dictionary<SFXType, AudioCue>();

        [SerializeField] private AudioEventChannelSO sfxChannel;
        public                   bool                IsMuted { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            OnInit();
        }

        private void OnInit()
        {
            this.clipDict = new Dictionary<SFXType, AudioCue>();
            for(int i = 0; i < types.Count; i++)
            {
                this.clipDict[types[i]] = clips[i];
            }

            this.sfxChannel.OnPlaySFX += PlaySFX;
        }

        private void OnDestroy()
        {
            sfxChannel.OnPlaySFX -= PlaySFX;
        }

        private void PlaySFX(SFXType type)
        {
            if(this.clipDict.TryGetValue(type, out var cue))
            {
                this.sfxSource.PlayOneShot(cue.clip, cue.volume);
            }
        }

        public void PlayMusic(AudioClip music)
        {
            this.musicSource.clip = music;
            this.musicSource.loop = true;
            this.musicSource.Play();
        }

        public void Unmute()
        {
            this.sfxSource.mute = false;
            this.musicSource.mute = false;
            IsMuted = false;
            UpdateMuteButtonVisual();
        }

        public void Mute()
        {
            this.sfxSource.mute = true;
            this.musicSource.mute = true;
            IsMuted = true;
            UpdateMuteButtonVisual();
        }

        private void UpdateMuteButtonVisual()
        {
            if(this.muteImage?.muteImage != null)
            {
                this.muteImage.muteImage.sprite = IsMuted
                    ? this.muteImage.muteSprite : this.muteImage.unmuteSprite;
            }
        }
    }
}

[System.Serializable]
public class AudioImage
{
    public Image muteImage;
    public Sprite muteSprite;
    public Sprite unmuteSprite;
}