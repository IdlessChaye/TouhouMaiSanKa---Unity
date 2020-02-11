using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class MusicManager : MonoBehaviour {
        // 要管理BGM单例、Voice单例、各种声音

        private Config config;

        public string VoiceIndex => voiceIndex;
        public string BGMIndex => bgmIndex;

        #region BGM
        private string bgmIndex;
        private AudioSource bgmAudioSource;
        #endregion
        #region Voice
        private string voiceIndex;
        private string characterName;
        private AudioSource voiceAudioSource;
        #endregion


        private void Awake() {
            config = PachiGrimoire.I.ConfigManager.Config;

            AudioSource[] audioSources = GetComponents<AudioSource>();
            if (audioSources != null) {
                foreach (var s in audioSources)
                    Destroy(s);
            }
            if (bgmAudioSource == null) {
                bgmAudioSource = gameObject.AddComponent<AudioSource>();
                bgmAudioSource.playOnAwake = false;
                bgmAudioSource.loop = true;
            }
            if (voiceAudioSource == null) {
                voiceAudioSource = gameObject.AddComponent<AudioSource>();
                bgmAudioSource.playOnAwake = false;
                bgmAudioSource.loop = true;
            }
        }

        public void BGMPlay(AudioClip clip, string bgmIndex) {
            if (clip == null)
                throw new System.Exception("MusicManager BGMPlay");
            bgmAudioSource.clip = clip;
            float volume = config.SystemVolume;
            volume *= config.BGMVolume;
            bgmAudioSource.volume = volume;
            bgmAudioSource.Play();
            this.bgmIndex = bgmIndex;
        }

        public void BGMStop() {
            bgmAudioSource.Stop();
            bgmIndex = null;
        }

        public void VoicePlay(string characterName, AudioClip clip, string voiceIndex) {
            if (clip == null)
                throw new System.Exception("MusicManager VoicePlay");
            voiceAudioSource.clip = clip;
            // 音量控制
            float volume = config.SystemVolume;
            volume *= config.VoiceVolume;
            this.characterName = characterName;
            if (string.IsNullOrEmpty(characterName) || config.CharacterNameList == null) {
                throw new System.Exception("MusicManager VoicePlay");
            } else {
                int index = config.CharacterNameList.IndexOf(characterName);
                if (index == -1) {
                    throw new System.Exception("MusicManager VoicePlay");
                } else {
                    volume *= config.VoiceVolumeValueList[index];
                }
            }
            voiceAudioSource.volume = volume;
            voiceAudioSource.Play();
            this.voiceIndex = voiceIndex;
        }

        public void VoiceStop() {
            voiceAudioSource.Stop();
            voiceIndex = null;
        }


    }
}