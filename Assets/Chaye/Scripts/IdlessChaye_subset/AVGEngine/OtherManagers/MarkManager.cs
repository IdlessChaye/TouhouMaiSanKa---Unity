using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class MarkManager : IRecordable {

        public List<string> MarkPlayerList => markPlayerList;
        public Dictionary<string, float> ValuePlayerDict => valuePlayerDict;
        public List<string> MarkStoryList => markStoryList;
        public Dictionary<string, float> ValueStoryDict => valueStoryDict;


        private List<string> markPlayerList = new List<string>();
        private Dictionary<string, float> valuePlayerDict = new Dictionary<string, float>();

        private List<string> markStoryList = new List<string>();
        private Dictionary<string, float> valueStoryDict = new Dictionary<string, float>();


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

        #endregion


        public void LoadPlayerData() {
            throw new System.NotImplementedException();
        }

        public void LoadStoryData() {
            throw new System.NotImplementedException();
        }
    }
}