using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    [System.Serializable]
    public struct KeyValuePair<TKey, TValue> {
        public TKey Key { get; private set; }
        public TValue Value { get; private set; }

        public KeyValuePair(TKey key, TValue value) {
            Key = key;
            Value = value;
        }

    }
}
