using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace IdlessChaye.IdleToolkit.AVGEngine {
    [Serializable]
    public class Config {
        public string Language;
        public string PlayerIdentifier;

        public List<string> CharacterNameList;

        public float SystemVolume;
        public float BGMVolume;
        public float VoiceVolume;
        public List<float> VoiceVolumeValueList;

        public bool IsPlayingVoiceAfterChangeLine;

        public float MessageSpeed;
        public float AutoMessageSpeed;
        public bool IsReadSkipOrAllSkipNot;

        public bool HasAnimationEffect;

        public float AlphaOfConsole;
    }
}