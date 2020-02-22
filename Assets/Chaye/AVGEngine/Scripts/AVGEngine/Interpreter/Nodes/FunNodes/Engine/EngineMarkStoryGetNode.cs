using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineMarkStoryGetNode : FunNode {

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("MarkStoryGet");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() {
            if (paraList.Count != 1)
                throw new System.Exception("EngineMarkStoryGetNode");

            string mark = paraList[0];
            bool isTrue = PachiGrimoire.I.MarkManager.MarkStoryGet(mark);
            if (isTrue == false) {
                PachiGrimoire.I.ScriptManager.IsAllTrue = false;
            }
        }


    }
}