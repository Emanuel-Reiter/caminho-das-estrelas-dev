using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicBox : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip explorationMusic;
    public AudioClip bossMusic;


    private void Start() {
        audioSource = GetComponent<AudioSource>();
        PlayerExplorationMusic();
    }

    public void PlayerExplorationMusic() {
        audioSource.volume = 0.25f;
        audioSource.clip = explorationMusic;
        audioSource.Play();
    }

    public void PlayBossMusic() {
        StartCoroutine(BossMusicCorroutine());
    }

    private IEnumerator BossMusicCorroutine() {
        audioSource.volume = Mathf.Lerp(audioSource.volume, 0.0f, 0.25f);
        yield return new WaitForSeconds(0.25f);
        audioSource.clip = bossMusic;
        audioSource.Play();
        audioSource.volume = Mathf.Lerp(0.0f, 0.333f, 0.333f);
    }
}
