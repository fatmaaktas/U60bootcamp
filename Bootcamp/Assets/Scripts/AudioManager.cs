using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource backgroundMusic;
    public AudioSource soundEffect;

    public AudioClip[] backgroundMusicClips;

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
    

    void Start()
    {
        AudioManager.instance.PlayBackgroundMusic(backgroundMusicClips, 0.5f);
    }


    public void PlayBackgroundMusic(AudioClip[] clips, float volume)
    {
        int randomIndex = Random.Range(0, clips.Length);
        AudioClip clip = clips[randomIndex];

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
