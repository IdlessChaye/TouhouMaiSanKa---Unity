﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {



    public class MySaveLoadButton : MonoBehaviour {

        public int number;

        public UILabel labelTitle;
        public UILabel labelDate;
        public UILabel labelContext;

        public string Title {
            get {
                return labelTitle.text;
            }
            set {
                labelTitle.text = value;
            }
        }

        public string Date {
            get {
                return labelDate.text;
            }
            set {
                labelDate.text = value;
            }
        }

        public string Context {
            get {
                return labelContext.text?.Substring(0,constData.NumberOfShowWords);
            }
            set {
                labelContext.text = value;
            }
        }

        private ConstData _constData;
        private ConstData constData {
            get {
                if (_constData == null) {
                    _constData = PachiGrimoire.I.constData;
                }
                return _constData;
            }
        }

        public void ClearLabel() {
            Title = null;
            Date = null;
            Context = null;
        }



    }
}