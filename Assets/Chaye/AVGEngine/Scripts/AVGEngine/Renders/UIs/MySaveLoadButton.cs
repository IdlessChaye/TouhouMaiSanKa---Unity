using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {



    public class MySaveLoadButton : MonoBehaviour {

        public int number;

        public UILabel labelTitle;
        public UILabel labelDate;
        public UILabel labelContext;
        public UILabel labelRecordNumber;

        public string RecordNumber {
            get {
                return labelRecordNumber.text;
            }
            set {
                labelRecordNumber.text = value;
            }
        }

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
                return labelContext.text;
            }
            set {
                if (value == null)
                    labelContext.text = "";
                else {
                    int maxNumberOfShowWords = constData.NumberOfShowWords;
                    int length = value.Length;
                    length = length <= maxNumberOfShowWords ? length : maxNumberOfShowWords;
                    labelContext.text = value.Substring(0, length);
                }
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