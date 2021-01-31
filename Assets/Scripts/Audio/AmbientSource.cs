using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmbientSource : MonoBehaviour
{
	[SerializeField]
	[Range(0f, 1f)]
	private float volume = 1f;
	[SerializeField]
	private List<AudioClip> ambientClips = new List<AudioClip>();
	[SerializeField]
	private List<AudioSource> audioSources = new List<AudioSource>();

	private float lastVolume;
	private int nextAudioSourceIndex = 0;

	private void Start()
	{
		lastVolume = volume;
		StartCoroutine(StartNextAmbientDelayed(0));
	}

	private void Update()
	{
		if(lastVolume != volume)
		{
			lastVolume = volume;
			SetVolume();
		}
	}

	private void SetVolume()
	{
		foreach(var audioSource in audioSources)
		{
			audioSource.volume = volume;
		}
	}

	private IEnumerator StartNextAmbientDelayed(float delayInSeconds)
	{
		while(true)
		{
			yield return new WaitForSeconds(delayInSeconds);

			AudioClip audioClip = ambientClips.ElementAt(UnityEngine.Random.Range(0, ambientClips.Count));

			AudioSource audioSource = audioSources.ElementAt(nextAudioSourceIndex);
			nextAudioSourceIndex = (nextAudioSourceIndex + 1) % audioSources.Count;

			audioSource.clip = audioClip;
			audioSource.Play();

			delayInSeconds = audioClip.length - 2f;
		}
	}
}
