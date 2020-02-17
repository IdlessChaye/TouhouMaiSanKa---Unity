using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineMarkStorySetNode : FunNode {

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("MarkStorySet");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() {
            if (paraList.Count != 1)
                throw new System.Exception("EngineMarkStorySetNode");

            string mark = paraList[0];
            PachiGrimoire.I.MarkManager.MarkStorySet(mark);
        }


    }
}