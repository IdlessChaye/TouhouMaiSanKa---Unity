using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ResBufferUnit<TKey, TValue> where TValue : class {
        public TKey Key { get; set; }
        public TValue Data { get; set; }
        public ResBufferUnit<TKey, TValue> Previous { get; set; }
        public ResBufferUnit<TKey, TValue> Next { get; set; }
    }
}