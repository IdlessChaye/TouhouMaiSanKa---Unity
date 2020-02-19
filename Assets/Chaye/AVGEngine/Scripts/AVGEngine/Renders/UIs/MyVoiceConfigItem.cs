using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace IdlessChaye.IdleToolkit.AVGEngine {

    /// <summary>
    /// 需要先设定CharacterName、Volume和Clip才能使用
    /// 使用者在退出的时候要调MusicManager的StopCharacterVoice函数
    /// </summary>
    public class MyVoiceConfigItem : MonoBehaviour {

        public UILabel labelName;
        public UISlider sliderValue;

        //[HideInInspector]
        //public Action onMyClick;
        //[HideInInspector]
        //public Action onMyValueChange;

        public int Index { get; set; } = -1;
        public AudioClip Clip { get; set; }
        public string CharacterName {
            get {
                return labelName.text;
            }
            set {
                labelName.text = value;
            }
        }
        public float Volume {
            get {
                return sliderValue.value;
            }
            set {
                sliderValue.value = value;
            }
        }

        private MusicManager musicManager;
        private Config config;
        public void Awake() {
            musicManager = PachiGrimoire.I.MusicManager;
            config = PachiGrimoire.I.ConfigManager.Config;
        }

        public void OnClickCallback() {
            if (Clip == null)
                return;
            musicManager.PlayCharacterVoice(CharacterName, Clip);
            //if (onMyClick != null)
            //    onMyClick.Invoke();
        }
        public void OnValueChangeCallback() {
            if (Index != -1)
                config.VoiceVolumeValueList[Index] = Volume;
            //if (onMyValueChange != null)
            //    onMyValueChange.Invoke();
        }

    }
}