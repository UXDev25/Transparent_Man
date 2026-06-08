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
        musicSource.clip = beggining;
        musicSource.Play();
    }

    public void StartBigasMusic() 
    {
        musicSource.Stop();
        musicSource.clip = bigas;
        musicSource.Play();
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
