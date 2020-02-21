using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineSystemDebugLogNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("SystemDebugLog");
            InterpretPart(context);
        }




        protected override void OnUpdateStageContext() {
            if (paraList.Count != 1)
                throw new System.Exception("EngineSystemDebugLogNode");
            string context = paraList[0];
            PachiGrimoire.I.DebugLog(context);
        }

    }
}