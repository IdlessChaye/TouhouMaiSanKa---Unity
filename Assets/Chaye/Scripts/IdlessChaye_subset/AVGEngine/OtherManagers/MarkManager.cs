using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class MarkManager {

        public List<string> MarkPlayerList => markPlayerList;
        public Dictionary<string, float> ValuePlayerDict => valuePlayerDict;
        public List<string> MarkStoryList => markStoryList;
        public Dictionary<string, float> ValueStoryDict => valueStoryDict;
        public string ChapterName => chapterName;



        private List<string> markPlayerList;
        private Dictionary<string, float> valuePlayerDict;

        private List<string> markStoryList;
        private Dictionary<string, float> valueStoryDict;
        private string chapterName;

        #region Player Functions
        public bool MarkPlayerGet(string mark) {
            return markPlayerList.Contains(mark);
        }

        public void MarkPlayerSet(string mark) {
            if (!markPlayerList.Contains(mark)) {
                markPlayerList.Add(mark);
            }
        }

        public float ValuePlayerGet(string varName) {
            if (valuePlayerDict.ContainsKey(varName)) {
                return valuePlayerDict[varName];
            } else {
                throw new System.Exception($"MarkManager ValuePlayerGet {varName}");
            }
        }

        public void ValuePlayerSet(string varName, float value) {
            if (valuePlayerDict.ContainsKey(varName)) {
                valuePlayerDict[varName] = value;
            } else {
                valuePlayerDict.Add(varName, value);
            }
        }

        #endregion


        #region Story Functions
        public bool MarkStoryGet(string mark) {
            return markStoryList.Contains(mark);
        }

        public void MarkStorySet(string mark) {
            if (!markStoryList.Contains(mark)) {
                markStoryList.Add(mark);
            }
        }

        public float ValueStoryGet(string varName) {
            if (valueStoryDict.ContainsKey(varName)) {
                return valueStoryDict[varName];
            } else {
                throw new System.Exception($"MarkManager ValueStoryGet {varName}");
            }
        }

        public void ValueStorySet(string varName, float value) {
            if (valueStoryDict.ContainsKey(varName)) {
                valueStoryDict[varName] = value;
            } else {
                valueStoryDict.Add(varName, value);
            }
        }

        public void ChapterNameSet(string chapterName) {
            this.chapterName = chapterName;
        }

        #endregion


        public void LoadPlayerRecord(List<string> markPlayerList, List<string> varNamePlayerList, List<float> varValuePlayerList) {
            this.markPlayerList = new List<string>(markPlayerList);
            this.valuePlayerDict = new Dictionary<string, float>();
            for (int i = 0; i < varNamePlayerList.Count; i++) {
                valuePlayerDict.Add(varNamePlayerList[i], varValuePlayerList[i]);
            }
        }

        public void LoadStoryRecord(List<string> markList, List<string> names, List<float> values, string chapterName) {
            if (markList != null)
                this.markStoryList = new List<string>(markList);
            else
                this.markStoryList.Clear();
            if (names != null) {
                valueStoryDict.Clear();
                for (int i = 0; i < names.Count; i++) {
                    valueStoryDict.Add(names[i], values[i]);
                }
            } else {
                valueStoryDict.Clear();
            }
            this.chapterName = chapterName;
        }


        public void InitializeStory() {
            markPlayerList = new List<string>();
            valuePlayerDict = new Dictionary<string, float>();

            markStoryList = new List<string>();
            valueStoryDict = new Dictionary<string, float>();
            chapterName = null;
        }

        public void FinalizeStory() {
            markPlayerList = null;
            valuePlayerDict = null;

            markStoryList = null;
            valueStoryDict = null;
            chapterName = null;
        }
    }
}