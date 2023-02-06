using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager> {
    [SerializeField] private Sound[] sounds;

    private void Awake() {
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
        DontDestroyOnLoad(gameObject);
    }

    public Sound GetSound(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound with name " + name + " not found.");
        }
        return s;
    }

    // Play one of the given sounds.
    // May result in slightly inconsistent behaviour if the same sound set is played multiple times;
    // should be refactored before using outside of jam code.
    public void Play(params string[] names) {
        string randomSound = names[UnityEngine.Random.Range(0, names.Length)];
        Play(randomSound);
    }

    public void Play(string name) {
        Sound s = GetSound(name);
        if (s == null) {
            return;
        }
        if (!s.interruptible && s.source.isPlaying) {
            return;
        }
        if (s.paused) {
            s.source.UnPause();
            s.paused = false;
        } else {
            s.source.Play();
        }
    }

    public void Pause(string name) {
        Sound s = GetSound(name);
        if (s == null) {
            return;
        }
        if (!s.source.isPlaying) {
            Debug.LogWarning("Pause() error: sound with name " + name + " is not playing.");
            return;
        }
        s.source.Pause();
        s.paused = true;
    }

    public void Stop(string name) {
        Sound s = GetSound(name);
        if (s == null) {
            return;
        }
        if (s.source.isPlaying) {
            s.source.Stop();
        }
    }

    public IEnumerator FadeOut(string name, float fadeDuration, bool pause) {
        Sound s = GetSound(name);
        if (s != null) {
            if (!s.source.isPlaying) {
                Debug.LogWarning("FadeOut() error: sound with name " + name + " is not playing.");
            } else {
                while(s.source.volume > 0) {
                    s.source.volume -= s.volume * Time.deltaTime / fadeDuration;
                    yield return null;
                }
                if (pause) {
                    Pause(name);
                } else {
                    Stop(name);
                }
                s.source.volume = s.volume;
            }
        }
    }

    public IEnumerator FadeIn(string name, float fadeDuration, bool unpause) {
        Sound s = GetSound(name);
        if (s != null) {
            s.source.volume = 0;
            if (!unpause) {
                s.paused = false;
            }
            Play(name);
            while(s.source.volume < s.volume) {
                s.source.volume += s.volume * Time.deltaTime / fadeDuration;
                yield return null;
            }
        }
    }

    public IEnumerator FadeTransition(string fadeOutName, float fadeOutDuration, bool pause, float transitionDelay, string fadeInName, float fadeInDuration, bool unpause) {
        Sound fadeOutSound = GetSound(fadeOutName);
        if (fadeOutSound != null && fadeOutSound.source.isPlaying) {
            StartCoroutine(FadeOut(fadeOutName, fadeOutDuration, pause));
            yield return new WaitForSeconds(fadeOutDuration);
            yield return new WaitForSeconds(transitionDelay);
        }
        StartCoroutine(FadeIn(fadeInName, fadeInDuration, unpause));
    }

    public void SetPitch(float pitch) {
        foreach (Sound s in sounds) {
            s.source.pitch = pitch;
        }
    }
}