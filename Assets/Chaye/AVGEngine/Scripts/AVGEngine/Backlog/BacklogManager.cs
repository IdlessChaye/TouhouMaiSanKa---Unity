using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class BacklogManager {
        public List<BacklogItem> BacklogItemList { get; private set; }
        public int Capacity => capacity;
        public int Count => count;
        public int Head => head;

        private int capacity;
        private int count;
        private int head;

        public BacklogManager(int capacity) {
            this.capacity = capacity;
        }

        public BacklogItem Seek(int offsetIndex) {
            int index = SeekIndex(offsetIndex);
            return BacklogItemList[index];
        }

        public void Push(string voiceIndex, string contextIndex, string imageIndex, string name) {
            BacklogItem item = new BacklogItem(voiceIndex, contextIndex, imageIndex, name);
            Push(item);
        }
        public void Push(BacklogItem item) {
            int index = (head + 1) % capacity;
            if (count < capacity) {
                if (count != index) { // index == 0, count == 0, count will increase
                    throw new System.Exception("BacklogManager Push");
                }
                count++; // count == index + 1
                BacklogItemList.Add(item);
            } else {
                BacklogItemList[index] = item;
            }
            head = index;
        }

        private int SeekIndex(int offsetIndex) {
            int index = head - offsetIndex;
            while (index < 0)
                index += count;
            return index % count;
        }

        public void LoadStoryRecord(List<BacklogItem> bs, int head, int capa, int count) {
            if(bs != null) {
                BacklogItemList = new List<BacklogItem>(bs);
            } else {
                BacklogItemList.Clear();
            }
            this.head = head;
            this.capacity = capa;
            this.count = count;
        }


        public void InitializeStory() {
            BacklogItemList = new List<BacklogItem>(capacity);
            count = 0;
            head = -1;
        }

        public void FinalizeStory() {
            BacklogItemList = null;
            count = 0;
            head = -1;
        }

        //private void Foreach() {
        //    BacklogManager backlogManager = PachiGrimoire.I.BacklogManager;
        //    for (int i = 0; i < backlogManager.Count; i++) {
        //        BacklogItem item = backlogManager.Seek(i);
        //    }
        //}
    }
}