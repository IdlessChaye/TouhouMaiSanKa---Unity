using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ConfigManager {
        public Config Config { get; set; } = new Config();

        public bool LoadConfigContext(string configJSON) {
            if (configJSON == null)
                return false;
            JsonUtility.FromJsonOverwrite(configJSON, Config);
            return true;
        }

        public void SaveConfigContext() {
            string configJSON = JsonUtility.ToJson(Config);
            PachiGrimoire.I.FileManager.SaveConfig(configJSON);
        }

        public void LoadDefaultConfig(bool isSetVoiceList = false) {
            string json = PachiGrimoire.I.FileManager.LoadDefaultConfig();
            Config newConfig = new Config();
            JsonUtility.FromJsonOverwrite(json, newConfig);
            if (isSetVoiceList) {
                Config.VoiceVolumeValueList = newConfig.VoiceVolumeValueList;
            } else {
                Config.Language = newConfig.Language; 
                Config.CharacterNameList = newConfig.CharacterNameList; 
                Config.SystemVolume = newConfig.SystemVolume; 
                Config.BGMVolume = newConfig.BGMVolume; 
                Config.SEVolume = newConfig.SEVolume; 
                Config.MessageSpeed = newConfig.MessageSpeed; 
                Config.AutoMessageSpeed = newConfig.AutoMessageSpeed; 
                Config.IsReadSkipOrAllSkipNot = newConfig.IsReadSkipOrAllSkipNot; 
                Config.VoiceVolume = newConfig.VoiceVolume; 
                Config.IsPlayingVoiceAfterChangeLine = newConfig.IsPlayingVoiceAfterChangeLine; 
                Config.HasAnimationEffect = newConfig.HasAnimationEffect; 
                Config.AlphaOfConsole = newConfig.AlphaOfConsole;
            }
        }
    }
}
