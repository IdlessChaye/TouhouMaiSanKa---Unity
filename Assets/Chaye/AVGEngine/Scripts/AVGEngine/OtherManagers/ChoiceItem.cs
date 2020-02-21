using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    [System.Serializable]
    public struct ChoiceItem {
        public string mark;
        public string dlIndex;
        public bool canBeSelected;
        public string onSelectedScirptContext;

        public ChoiceItem(string mark,
         string dlIndex,
         bool canBeSelected,
         string onSelectedScirptContext) {
            this.mark = mark;
            this.dlIndex = dlIndex;
            this.canBeSelected = canBeSelected;
            this.onSelectedScirptContext = onSelectedScirptContext;
        }
    }
}