using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}