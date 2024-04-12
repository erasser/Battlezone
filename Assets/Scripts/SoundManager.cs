using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Sm;
    public AudioClip shotEffect;
    public AudioClip projectileDestroyEffect;
    public AudioClip tankDestroyEffect;
    public AudioClip teleportEffect;

    void Start()
    {
        Sm = this;
    }

    public void PlayClip(AudioClip clip, GameObject source)
    {
        AudioSource audioSource = new GameObject("audio source - " + source.name, typeof(AudioSource)).GetComponent<AudioSource>();
        audioSource.transform.position = source.transform.position;
        audioSource.PlayOneShot(clip);
        StartCoroutine(DestroySource(audioSource, clip.length));
    }

    IEnumerator DestroySource(AudioSource source, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(source.gameObject);
    }
}
