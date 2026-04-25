using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("UI Audios")]
    [SerializeField] public AudioClip greenAudio;
    [SerializeField] public AudioClip redAudio;
    [SerializeField] public AudioClip yellowAudio;
    [SerializeField] public AudioClip clickRightAns;
    [SerializeField] public AudioClip clickWrongAns;

    [Header("Narration Audios")]
    //[SerializeField] public AudioClip selectedA;
    //[SerializeField] public AudioClip selectedB;
    //[SerializeField] public AudioClip selectedC;
    //[SerializeField] public AudioClip selectedD;
    [SerializeField] public AudioClip lockOption;
    [SerializeField] public AudioClip selectedRightAns;
    [SerializeField] public AudioClip selectedWrongAns;

    [Header("LifeLine Audios")]
    [SerializeField] public AudioClip fiftyfifty;
    [SerializeField] public AudioClip SkipQues;
    [SerializeField] public AudioClip Audience;

    public float audioEndTime;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void Stop()
    {
        audioSource.Stop();
    }
}
