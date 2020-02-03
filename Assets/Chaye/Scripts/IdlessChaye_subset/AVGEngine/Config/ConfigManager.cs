using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ConfigManager {
        private Config config = new Config();
        public Config Config => config;

        public bool LoadConfigContext(string configJSON) {
            if (configJSON == null)
                return false;
            JsonUtility.FromJsonOverwrite(configJSON, config);
            return true;
        }

        public void SaveConfigContext() {
            string configJSON = JsonUtility.ToJson(config);
            PachiGrimoire.I.FileManager.SaveConfig(configJSON);
        }
    }
}
