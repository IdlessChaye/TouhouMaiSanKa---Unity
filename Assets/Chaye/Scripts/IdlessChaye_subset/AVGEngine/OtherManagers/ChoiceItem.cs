﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    [System.Serializable]
    public struct ChoiceItem {

        public string Mark => mark;
        public string DLIndex => dlIndex;
        public bool CanBeSelected => canBeSelected;
        public string OnSelectedScirptContext => onSelectedScirptContext;
        public int Number => number;

        private string mark;
        private string dlIndex;
        private bool canBeSelected;
        private string onSelectedScirptContext;
        private int number;

        public ChoiceItem(string mark,
         string dlIndex,
         bool canBeSelected,
         string onSelectedScirptContext,
         int number) {
            this.mark = mark;
            this.dlIndex = dlIndex;
            this.canBeSelected = canBeSelected;
            this.number = number;
            this.onSelectedScirptContext = onSelectedScirptContext;
        }
    }
}