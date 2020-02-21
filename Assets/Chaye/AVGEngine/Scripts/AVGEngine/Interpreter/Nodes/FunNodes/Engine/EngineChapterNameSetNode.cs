using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineChapterNameSetNode : FunNode {

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("ChapterNameSet");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() {
            if (paraList.Count != 1)
                throw new System.Exception("EngineChapterNameSetNode");

            string chapterName = paraList[0];
            PachiGrimoire.I.MarkManager.ChapterNameSet(chapterName);
        }


    }
}