using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineScriptReplaceAddNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("ScriptReplace");
            InterpretPart(context);
        }




        protected override void OnUpdateStageContext() {
            if (paraList.Count != 2)
                throw new System.Exception("EngineScriptReplaceNode");
            PachiGrimoire.I.ScriptManager.ScriptReplaceAdd(paraList[0], paraList[1]);
        }

    }
}