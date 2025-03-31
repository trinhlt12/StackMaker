using UnityEngine;

[CreateAssetMenu(menuName = "Audio/AudioCue")]
public class AudioCue : ScriptableObject
{
    public AudioClip clip;
    public float     volume = 1f;
}