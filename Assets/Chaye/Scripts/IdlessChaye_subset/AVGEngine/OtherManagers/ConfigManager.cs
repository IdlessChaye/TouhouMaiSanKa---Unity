using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ConfigManager {
        private string configContext;
        public string ConfigContext => configContext;

        public bool CanProcessConfigContext(string context) {
            if (context == null)
                return false;

            configContext = context;
            return true;
        }


    }
}
