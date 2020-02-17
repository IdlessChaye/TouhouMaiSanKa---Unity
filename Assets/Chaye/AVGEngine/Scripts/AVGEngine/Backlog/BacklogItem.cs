using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    [System.Serializable]
    public class BacklogItem {
        public string voiceIndex;
        public string contextIndex;
        public string imageIndex;
        public string name;

        public BacklogItem(string voiceIndex,
         string contextIndex,
         string imageIndex,
         string name) {
            this.voiceIndex = voiceIndex;
            this.contextIndex = contextIndex;
            this.imageIndex = imageIndex;
            this.name = name;
        }
    }
}