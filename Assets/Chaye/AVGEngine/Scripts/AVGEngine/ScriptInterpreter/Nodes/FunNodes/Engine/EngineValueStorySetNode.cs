using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineValueStorySetNode : FunNode {

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("ValueStorySet");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() {
            if (paraList.Count != 2)
                throw new System.Exception("EngineValueStorySetNode");

            string name = paraList[0];
            float value = float.Parse(paraList[1]);
            PachiGrimoire.I.MarkManager.ValueStorySet(name, value);
        }


    }
}