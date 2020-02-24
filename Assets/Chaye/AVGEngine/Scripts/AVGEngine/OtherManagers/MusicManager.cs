using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class MusicManager : MonoBehaviour {
        // 要管理BGM单例、Voice单例、各种声音

        public string BGMIndex => bgmIndex;
        public string VoiceIndex => voiceIndex;
        public string CharacterName => characterName;


        private Config config;

        #region BGM
        private string bgmIndex;
        private AudioSource bgmAudioSource;
        #endregion
        #region Voice
        private string voiceIndex;
        private string characterName;
        private AudioSource voiceAudioSource;
        #endregion
        #region Backup
        private AudioSource backupAudioSource;
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
                bgmAudioSource.loop = false;
            }
            if (backupAudioSource == null) {
                backupAudioSource = gameObject.AddComponent<AudioSource>();
                backupAudioSource.playOnAwake = false;
                backupAudioSource.loop = false;
            }
        }

        public void BGMPlay(AudioClip clip, string bgmIndex, bool hasEffect = true) {
            if (clip == null)
                throw new System.Exception("MusicManager BGMPlay");

            this.bgmIndex = bgmIndex;
            bgmAudioSource.clip = clip;
            float volume = config.SystemVolume;
            volume *= config.BGMVolume;
            bgmAudioSource.volume = volume;
            if (hasEffect) {
                bgmAudioSource.Play();
            }
        }


        public void BGMStop(bool hasEffect = true) {
            bgmIndex = null;
            if (hasEffect) {
                bgmAudioSource.Stop();
            }
        }


        public void VoicePlay(string characterName, AudioClip clip, string voiceIndex, bool hasEffect = true) {
            if (string.IsNullOrEmpty(characterName)) {
                throw new System.Exception("MusicManager VoicePlay");
            };
            this.characterName = characterName;
            this.voiceIndex = voiceIndex;
            voiceAudioSource.clip = clip;
            float volume = config.SystemVolume;
            volume *= config.VoiceVolume;
            int index = config.CharacterNameList.IndexOf(characterName);
            if (index == -1) {
                throw new System.Exception("MusicManager VoicePlay");
            } else {
                volume *= config.VoiceVolumeValueList[index];
            }
            voiceAudioSource.volume = volume;
            if (hasEffect) {
                voiceAudioSource.Play();
            }
        }

        public void VoicePlay() {
            if (voiceIndex != null) {
                voiceAudioSource.Play();
            }
        }

        public void VoiceStop(bool hasEffect = true) {
            voiceIndex = null;
            characterName = null;
            if (hasEffect) {
                voiceAudioSource.Stop();
            }
        }


        public void LoadStoryRecord(string bgmIndex, string voiceIndex, string characterName) {
            if (!string.IsNullOrEmpty(bgmIndex)) {
                AudioClip bgmClip = PachiGrimoire.I.ResourceManager.Get<AudioClip>(bgmIndex);
                BGMPlay(bgmClip, bgmIndex);
            } else {
                this.bgmIndex = null;
            }
            if (!string.IsNullOrEmpty(voiceIndex) && !string.IsNullOrEmpty(characterName)) {
                AudioClip voiceClip = PachiGrimoire.I.ResourceManager.Get<AudioClip>(voiceIndex);
                VoicePlay(characterName, voiceClip, voiceIndex, false);
            } else {
                this.characterName = null;
                this.voiceIndex = null;
            }
        }


        public void PlayCharacterVoice(string characterName, AudioClip clip) {
            if (clip == null || string.IsNullOrEmpty(characterName)) {
                throw new System.Exception("MusicManager PlayCharacterVoice");
            }
            backupAudioSource.clip = clip;
            float volume = config.SystemVolume;
            volume *= config.VoiceVolume;
            int index = config.CharacterNameList.IndexOf(characterName);
            if (index == -1) {
                throw new System.Exception("MusicManager PlayCharacterVoice");
            } else {
                volume *= config.VoiceVolumeValueList[index];
            }
            backupAudioSource.volume = volume;
            backupAudioSource.Play();
        }

        public void StopCharacterVoice() {
            backupAudioSource?.Stop();
        }





        public void InitializeStory() {
            BGMStop();
            VoiceStop();
            voiceIndex = null;
            characterName = null;
            bgmIndex = null;
        }

        public void FinalizeStory() {
            BGMStop();
            VoiceStop();
            voiceIndex = null;
            characterName = null;
            bgmIndex = null;
        }



    }
}