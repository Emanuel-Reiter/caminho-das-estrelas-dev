using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public Vector2 pitchMinMax = Vector2.one;
    public float selfDestroyTime = 2.0f;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(pitchMinMax.x, pitchMinMax.y);
        audioSource.Play();
        Destroy(this.gameObject, selfDestroyTime);
    }
}
