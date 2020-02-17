using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineScriptIfThenElseNode : FunNode {

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("ScriptIfThenElse");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() {
            if (paraList.Count != 3)
                throw new System.Exception("EngineScriptIfThenElseNode");
            PachiGrimoire.I.ScriptManager.ScriptIfThenElse(paraList[0], paraList[1], paraList[2]);
        }


    }
}