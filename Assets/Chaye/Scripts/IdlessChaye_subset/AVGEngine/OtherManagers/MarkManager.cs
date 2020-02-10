using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class MarkManager {

        private List<string> markList = new List<string>();
        private List<string> varNameList = new List<string>();
        private List<int> varValueList = new List<int>();

        public void LoadData(List<string> markList, List<string> varNameList, List<int> varValueList) {
            if((varNameList ==null && varValueList == null) || varNameList?.Count == varValueList?.Count) {
                if(varNameList != null) {
                    this.varNameList = varNameList;
                    this.varValueList = varValueList;
                }
            } else {
                throw new System.Exception("MarkManager LoadData");
            }
            if(markList != null) {
                this.markList = markList;
            }
        }

        public bool GetMark(string mark) {
            return markList.Contains(mark);
        }

        public void SetMark(string mark) {
            if(!markList.Contains(mark)) {
                markList.Add(mark);
            }
        }

        public int GetValue(string varName) {
            if (varName.Contains(varName)) {
                return varValueList[varNameList.IndexOf(varName)];
            } else {
                throw new System.Exception($"MarkManager GetValue varName: {varName}");
            }
        }

        public void SetValue(string varName, int value) {
            if (varNameList.Contains(varName)) {
                varValueList[varNameList.IndexOf(varName)] = value;
            } else {
                varNameList.Add(varName);
                varValueList.Add(value);
            }
        }
    }
}