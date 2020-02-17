using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class DialogResBufferIndexer<TValue> : BaseResBufferIndexer<string,TValue> where TValue : class{

        public DialogResBufferIndexer(int bufferMaxCount) : base (bufferMaxCount) { }

        protected override void DestroyValue(TValue value) {
            List<string> list = value as List<string>;
            list?.Clear();
            list = null;
        }

        protected override TValue LoadValue(string finalIndex) {
            if (finalIndex.Contains("StreamingAssets"))
                return FileManager.ReadTXTFileAsList(finalIndex) as TValue;
            else
                return FileManager.LoadDialogAsset(finalIndex) as TValue;
        }
    }
}