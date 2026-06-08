using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip beggining;
    public AudioClip bigas;
    public AudioClip final;
    public AudioClip hit;

    private bool _stopMusic = false;

    private void Start() 
    { 
        _stopMusic = false;
        musicSource.clip = beggining;
        StartMusic();
    }

    private void Update()
    {
        if (GameManager.Instance.GameWon && !_stopMusic)
        {
            musicSource.clip = final;
            musicSource.PlayOneShot(final);
            _stopMusic = true;
        }
        if (GameManager.Instance.PreGameWon && musicSource.clip != final)
        {
            musicSource.Stop();
        }
    }

    public void StartMusic() 
    {
        musicSource.Stop();
        musicSource.PlayOneShot(beggining);
    }

    public void StartBigasMusic() 
    {
        musicSource.Stop();
        musicSource.PlayOneShot(bigas);
    }

    public void PlayHitSFX() 
    { 
        sfxSource.PlayOneShot(hit);
    }

    public void FadeOutMusic()
    {
        musicSource.Stop();
    }
}
