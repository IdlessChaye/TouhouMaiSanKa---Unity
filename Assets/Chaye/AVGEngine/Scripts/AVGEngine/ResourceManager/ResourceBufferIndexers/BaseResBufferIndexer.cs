using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public abstract class BaseResBufferIndexer<TKey, TValue> where TValue : class {
        public int MaxCount { get; private set; }
        public int Count { get; private set; }
        public bool IsEmpty => Count == 0;
        public ResBufferUnit<TKey, TValue> First { get; private set; }
        public ResBufferUnit<TKey, TValue> Last { get; private set; }



        private FileManager fileManager;
        protected FileManager FileManager {
            get {
                if (fileManager == null) {
                    fileManager = PachiGrimoire.I.FileManager;
                }
                return fileManager;
            }
        }

        protected Dictionary<TKey, ResBufferUnit<TKey, TValue>> bufferDict { get; private set; }



        public BaseResBufferIndexer(int maxCount) {
            this.MaxCount = maxCount;
            bufferDict = new Dictionary<TKey, ResBufferUnit<TKey, TValue>>(MaxCount);
        }

        public TValue Get(TKey key, string finalIndex) {
            Debug.Log("Get finalIndex: "+finalIndex);
            if (bufferDict.ContainsKey(key)) {
                Debug.Log("BufferIndexer Contain. Get old data.");
                ResBufferUnit<TKey, TValue> unit = bufferDict[key];
                if (First != unit) {
                    RemoveUnit(unit);
                    AddUnit(unit);
                }
                return unit.Data;
            } else {
                Debug.Log("BufferIndexer Not Contain. Set new data.");
                if (AddNewValueByFinalIndex(key, finalIndex) == true) {
                    return bufferDict[key].Data;
                } else {
                    throw new System.Exception("ResBufferIndexer::AddNewValue出问题了");
                }
            }
        }

        public bool ForceAdd(TKey key, TValue value) {
            if (value == null) {
                return false;
            }
            ResBufferUnit<TKey, TValue> newUnit = new ResBufferUnit<TKey, TValue> {
                Key = key,
                Data = value
            };
            return AddUnit(newUnit, true);
        }



        protected abstract TValue LoadValue(string finalIndex);

        protected abstract void DestroyValue(TValue value);



        private bool BeLargerSize(int largerMaxCount) {
            if (largerMaxCount <= MaxCount)
                return false;
            MaxCount = largerMaxCount;
            Debug.Log("Size!" + MaxCount);
            return true;
        }

        private bool AddUnit(ResBufferUnit<TKey, TValue> unit, bool beLarger = false) {
            if (unit == null || unit.Data == null)
                return false;
            unit.Next = First;
            Debug.Log("Data : "+unit.Data);
            if (Count == 0) {
                Count++;
                First = unit;
                Last = unit;
            } else if (Count < MaxCount) {
                Count++;
                First.Previous = unit;
                First = unit;
            } else {
                if (beLarger) {
                    BeLargerSize(Count + 1);
                } else {
                    RemoveAndDestroyUnit(Last);
                }
                Count++;
                First.Previous = unit;
                First = unit;
            }
            bufferDict.Add(unit.Key, unit);
            return true;
        }

        private bool AddNewValueByFinalIndex(TKey key, string finalIndex) {
            TValue value = LoadValue(finalIndex);
            if (value == null) {
                throw new System.Exception($"ResourceBufferIndexer::AddValueByPath::LoadValueByPath出问题了, finalIndex : {finalIndex}");
            }
            ResBufferUnit<TKey, TValue> newUnit = new ResBufferUnit<TKey, TValue> {
                Key = key,
                Data = value
            };
            return AddUnit(newUnit);
        }

        private ResBufferUnit<TKey, TValue> RemoveUnit(ResBufferUnit<TKey, TValue> unit) {
            if (unit == null || unit.Data == null)
                return null;
            if (bufferDict.ContainsValue(unit) == false) {
                return null;
            }
            if (bufferDict.Remove(unit.Key) == false) {
                throw new System.Exception("这还能出错?");
            }
            ResBufferUnit<TKey, TValue> previous = unit.Previous;
            ResBufferUnit<TKey, TValue> next = unit.Next;
            if (previous != null) {
                previous.Next = next;
            } else {
                First = next;
            }
            if (next != null) {
                next.Previous = previous;
            } else {
                Last = previous;
            }
            Count--;
            return unit;
        }

        private void RemoveAndDestroyUnit(ResBufferUnit<TKey, TValue> unit) {
            Debug.Log("RemoveAndDestroyUnit :" + unit.Data);
            ResBufferUnit<TKey, TValue> oldUnit = RemoveUnit(unit);
            if (oldUnit == null) {
                throw new System.Exception("咋回事儿?");
            }
            TValue value = unit.Data;
            unit.Data = null;
            DestroyValue(value);
        }
    }
}