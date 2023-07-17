using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource backgroundMusic;
    public AudioSource soundEffect;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBackgroundMusic(AudioClip clip, float volume)
    {
        backgroundMusic.clip = clip;
        backgroundMusic.volume = volume;
        backgroundMusic.Play();
    }

    public void PlaySkillSound(AudioClip clip, float volume)
    {
        soundEffect.PlayOneShot(clip, volume);
    }

    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }
}
