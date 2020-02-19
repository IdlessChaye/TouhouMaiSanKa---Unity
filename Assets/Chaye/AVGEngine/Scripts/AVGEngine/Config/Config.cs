using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    [Serializable]
    public class Config {
        public string Language; // Set by system

        public List<string> CharacterNameList; // Default

        public float SystemVolume; // Slider
        public float BGMVolume; // Slider
        public float SEVolume; // Slider

        public float MessageSpeed; // Slider
        public float AutoMessageSpeed; // Slider
        public bool IsReadSkipOrAllSkipNot; // Toggle
        

        public float VoiceVolume; // Slider
        public List<float> VoiceVolumeValueList; // Sliders
        public bool IsPlayingVoiceAfterChangeLine; // Toggle

        public bool HasAnimationEffect; // Toggle

        public float AlphaOfConsole;

    }
}