using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineMarkPlayerSetNode : FunNode {

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("MarkPlayerSet");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() {
            if (paraList.Count != 1)
                throw new System.Exception("EngineMarkPlayerSetNode");

            string mark = paraList[0];
            PachiGrimoire.I.MarkManager.MarkPlayerSet(mark);
        }


    }
}