using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class BgImageResBufferIndexer<TValue> : BaseResBufferIndexer<string, TValue> where TValue : class {
        public BgImageResBufferIndexer(int maxCount) : base(maxCount) { }

        protected override void DestroyValue(TValue value) {
            throw new System.NotImplementedException();
        }

        protected override TValue LoadValueByPath(string path) {
            throw new System.NotImplementedException();
        }
    }
}