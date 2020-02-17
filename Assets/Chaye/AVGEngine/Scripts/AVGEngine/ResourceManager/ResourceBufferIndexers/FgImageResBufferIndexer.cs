using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class FgImageResBufferIndexer<TValue> : BaseResBufferIndexer<string,TValue> where TValue:class {
        private int destroyValueCount = 0;
        public FgImageResBufferIndexer(int maxCount) : base(maxCount) { }
        protected override void DestroyValue(TValue value) {
            if (destroyValueCount++ >= 20) {
                destroyValueCount = 0;
                Resources.UnloadUnusedAssets();
                System.GC.Collect();
            }
        }

        protected override TValue LoadValue(string finalIndex) {
            Debug.Log("LoadValue finalIndex: " + finalIndex);
            if (finalIndex.Contains("StreamingAssets"))
                return FileManager.GetTexture2DFromPath(finalIndex) as TValue;
            else
                return FileManager.LoadFgImageAsset(finalIndex) as TValue;
        }
    }
}