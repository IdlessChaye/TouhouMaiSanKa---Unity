using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class VoiceAssetBundleResBufferIndexer<TValue> : BaseResBufferIndexer<string, TValue> where TValue : class {
        public VoiceAssetBundleResBufferIndexer(int maxCount) : base(maxCount) { }

        protected override void DestroyValue(TValue value) {
            AssetBundle ab = value as AssetBundle;
            ab.Unload(false);
            Resources.UnloadUnusedAssets();
        }

        protected override TValue LoadValue(string chapter) {
            return FileManager.GetAssetBundleByName(chapter) as TValue;
        }
    }
}
