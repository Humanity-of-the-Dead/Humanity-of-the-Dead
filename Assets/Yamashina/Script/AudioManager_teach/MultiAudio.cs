using UnityEngine;
using UnityEngine.Audio;

public class MultiAudioManager : MonoBehaviour
{
    public AudioClip[] audioClipsBGM; // Array for multiple BGM clips
    public AudioClip[] audioClipsSE;  // Array for multiple SE clips

    private AudioSource bgmSource;
    private AudioSource seSource;

    // Audio Mixer Groups to assign different mixer settings
    public AudioMixerGroup bgmMixerGroup;
    public AudioMixerGroup seMixerGroup;
    public AudioMixerGroup uiMixerGroup;

    private void Start()
    {
        bgmSource = GameObject.FindWithTag("BGM").GetComponent<AudioSource>();
        seSource = GameObject.FindWithTag("SE").GetComponent<AudioSource>();

        // Assign mixer groups to the audio sources
        if (bgmSource != null) bgmSource.outputAudioMixerGroup = bgmMixerGroup;
        if (seSource != null) seSource.outputAudioMixerGroup = seMixerGroup;
    }

    // Method to play a selected BGM by index
    public void ChooseSongs_BGM(int index)
    {
        if (index >= 0 && index < audioClipsBGM.Length)
        {
            bgmSource.clip = audioClipsBGM[index];
            if (bgmSource.clip != null)
            {
                bgmSource.Play();
                Debug.Log("Playing BGM: " + bgmSource.clip.name);
            }
            else
            {
                Debug.LogWarning("BGM clip not set.");
            }
        }
        else
        {
            Debug.LogWarning("BGM index out of range.");
        }
    }

    // Method to play a selected SE by index
    public void ChooseSongs_SE(int index)
    {
        if (index >= 0 && index < audioClipsSE.Length)
        {
            seSource.clip = audioClipsSE[index];

            // Check if the SE clip name contains "UI" to assign UI mixer group
            if (audioClipsSE[index].name.Contains("UI"))
            {
                seSource.outputAudioMixerGroup = uiMixerGroup;
            }
            else
            {
                seSource.outputAudioMixerGroup = seMixerGroup;
            }

            // Play SE as a one-shot sound
            seSource.PlayOneShot(seSource.clip);
            Debug.Log("Playing SE: " + seSource.clip.name);
        }
        else
        {
            Debug.LogWarning("SE index out of range.");
        }
    }
}
