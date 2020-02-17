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
    }
}
