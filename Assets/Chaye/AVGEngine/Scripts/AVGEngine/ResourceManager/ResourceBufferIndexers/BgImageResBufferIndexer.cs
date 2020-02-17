using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class BgImageResBufferIndexer<TValue> : BaseResBufferIndexer<string, TValue> where TValue : class {
        private int destroyValueCount = 0;
        public BgImageResBufferIndexer(int maxCount) : base(maxCount) { }
        protected override void DestroyValue(TValue value) {
            if (destroyValueCount++ >= 10) {
                destroyValueCount = 0;
                Resources.UnloadUnusedAssets();
                System.GC.Collect();
            }
        }

        protected override TValue LoadValue(string finalIndex) {
            Debug.Log("LoadValue finalIndex: "+finalIndex);
            if (finalIndex.Contains("StreamingAssets"))
                return FileManager.GetTexture2DFromPath(finalIndex) as TValue;
            else
                return FileManager.LoadBgImageAsset(finalIndex) as TValue;
        }
    }
}