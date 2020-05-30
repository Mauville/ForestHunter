using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	[Header ("Sounds")]
	public Sound[] sounds;
	private Sound s;

	void Awake()
	{
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
            s.source.volume=s.volume;
		}
	}
    void Start() {
        Play("Theme");
    }
	private void Update() {
		if(PauseManager.gameIsPaused){
			s.source.volume=0.25f;
		}else{
			s.source.volume=0.5f;
		}
	}

	public void Play(string sound)
	{
		s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}
		s.source.Play();
	}

	public void Pause(string sound){
		s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.Pause();
	}

}