using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineMarkPlayerGetNode : FunNode {

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("MarkPlayerGet");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() {
            if (paraList.Count != 1)
                throw new System.Exception("EngineMarkPlayerGetNode");

            string mark = paraList[0];
            bool isTrue = PachiGrimoire.I.MarkManager.MarkPlayerGet(mark);
            if(isTrue == false) {
                PachiGrimoire.I.ScriptManager.IsAllTrue = false;
            }
        }


    }
}