﻿using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Audio
{
    public class AudioAssistant : MonoBehaviour
    {
        public static AudioAssistant Instance { get; private set; }
        private static readonly List<TypeSound> MixBuffer = new List<TypeSound>(32);
        private static readonly float MixBufferClearDelay = 0.05f;
        private static readonly float TimeUnScaleDentaTime = 0.02f;

        public string currentTrack;

        [SerializeField]
        private AudioSource music;

        [SerializeField]
        private AudioSource sfx;

        private SoundConfig _soundCfg;

        public float MusicVolume
        {
            get
            {
                if (GameManager.Instance)
                    return GameManager.Instance.GameData.setting.bgMusicVolume * _soundCfg.bgMusicVolume;
                
                return _soundCfg.bgMusicVolume;
            }
        }

        private void OnEnable()
        {
            InitConstant();
            EventGlobalManager.Instance.OnUpdateSetting.AddListener(UpdateSoundSetting);
        }

        private void OnDestroy()
        {
            if (EventGlobalManager.Instance == null)
                return;

            EventGlobalManager.Instance.OnUpdateSetting.RemoveListener(UpdateSoundSetting);
        }

        private void UpdateSoundSetting()
        {
            if (GameManager.Instance && GameManager.Instance.GameData != null)
            {
                sfx.volume = _soundCfg.sfxVolume * GameManager.Instance.GameData.setting.sfxVolume;
                music.volume = _soundCfg.bgMusicVolume * GameManager.Instance.GameData.setting.bgMusicVolume;
            }
        }

        public void InitConstant()
        {
            if (Instance == null)
                Instance = this;

            _soundCfg = ConfigManager.Instance.soundConfig;

            StartCoroutine(MixBufferRoutine());
        }

        // Coroutine responsible for limiting the frequency of playing sounds
        private IEnumerator MixBufferRoutine()
        {
            float time = 0;

            while (true)
            {
                time += TimeUnScaleDentaTime;
                yield return null;
                if (time >= MixBufferClearDelay)
                {
                    MixBuffer.Clear();
                    time = 0;
                }
            }
        }

        // Launching a music track
        public void PlayMusic(string trackName, float delay = .3f, float delayStartFadeOut = 0f)
        {
            if (trackName != string.Empty)
                currentTrack = trackName;

            AudioClip to = null;
            if (trackName != string.Empty)
            {
                var listTrack = new List<Bgm>(1);

                foreach (var track in _soundCfg.listBgm)
                    if (track.name == trackName)
                        listTrack.Add(track);

                var random = Random.Range(0, listTrack.Count);
                to = listTrack[random].track;
            }

            if (Instance != null && to != null)
                StartCoroutine(Instance.CrossFade(to, delay, delayStartFadeOut));
        }

        public void PlayRandomMusic()
        {
            PlayMusic(_soundCfg.listBgm.PickRandom().name);
        }

        // A smooth transition from one to another music
        private IEnumerator CrossFade(AudioClip to, float delay = .3f, float delayStartFadeOut = 0f)
        {
            var countDownDelay = delay;
            if (music.clip != null)
                while (countDownDelay > 0)
                {
                    music.volume = countDownDelay * MusicVolume;
                    countDownDelay -= TimeUnScaleDentaTime;
                    yield return null;
                }

            music.clip = to;
            if (to == null)
            {
                music.Stop();
                yield break;
            }

            yield return Yielders.Get(delayStartFadeOut);
            countDownDelay = 0;
            if (!music.isPlaying)
                music.Play();
            while (countDownDelay < delay)
            {
                music.volume = countDownDelay * MusicVolume;
                countDownDelay += TimeUnScaleDentaTime;
                yield return null;
            }

            music.volume = MusicVolume;
        }

        public static void Shot(TypeSound typeSound)
        {
            if (typeSound != TypeSound.None && !MixBuffer.Contains(typeSound))
            {
                MixBuffer.Add(typeSound);
                if (Instance != null && Instance.sfx != null && Instance._soundCfg.GetAudio(typeSound) != null)
                    Instance.sfx.PlayOneShot(Instance._soundCfg.GetAudio(typeSound));
            }
        }

        public static float GetLength(TypeSound typeSound)
        {
            if (typeSound != TypeSound.None)
                return Instance._soundCfg.GetAudio(typeSound).length;
            return 0f;
        }

        public void FadeOutMusic()
        {
            music.volume = 0.1f;
        }

        public void FadeInMusic()
        {
            music.volume = MusicVolume;
        }

        public void Pause()
        {
            music.Pause();
        }

        public void PlayAgain()
        {
            music.UnPause();
        }
    }
}