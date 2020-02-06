using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ScriptResBufferIndexer<TValue> : BaseResBufferIndexer<string,TValue> where TValue:class{

        public ScriptResBufferIndexer(int bufferMaxCount) :base(bufferMaxCount) { }

        protected override void DestroyValue(TValue value) {
            return;
        }

        protected override TValue LoadValue(string finalIndex) {
            if (finalIndex.Contains("StreamingAssets"))
                return FileManager.ReadTXTFile(finalIndex) as TValue;
            else
                return FileManager.LoadScriptAsset(finalIndex) as TValue;
        }
    }
}