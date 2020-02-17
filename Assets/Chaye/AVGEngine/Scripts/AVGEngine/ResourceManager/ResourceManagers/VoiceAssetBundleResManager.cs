using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class VoiceAssetBundleResManager<TValue> where TValue : class {
        private VoiceAssetBundleResBufferIndexer<TValue> bufferIndexer;
        private VoiceAssetBundleConfigManager configManager;

        public VoiceAssetBundleResManager(int bufferMaxCount) {
            bufferIndexer = new VoiceAssetBundleResBufferIndexer<TValue>(bufferMaxCount);
        }

        public TValue Get(string subIndex) {
            string key = subIndex;
            string chapter = configManager.GetChapter(subIndex);
            return bufferIndexer.Get(key, chapter);
        }

        public bool LoadConfigManager(VoiceAssetBundleConfigManager configManager) {
            this.configManager = configManager;
            return true;
        }
    }
}
