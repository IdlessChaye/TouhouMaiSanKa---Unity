using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class StageContextManager {
        public string scriptPointerScriptName;
        public int scriptPointerLineNumber;
        public List<string> scriptReplaceKeys = new List<string>();
        public List<string> scriptReplaceValues = new List<string>();
    }
}