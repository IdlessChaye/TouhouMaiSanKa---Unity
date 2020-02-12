using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineChoiceCreateNode : FunNode {

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("ChoiceCreate");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() {
            if (paraList.Count == 0 || paraList.Count % 3 != 0)
                throw new System.Exception("EngineChoiceCreateNode");

            List<ChoiceItem> choiceItemList = new List<ChoiceItem>();
            string mark;
            string dlIndex;
            bool canBeSelected;
            int number = 0;
            for (int i = 0; i < paraList.Count; i += 3) {
                mark = paraList[i];
                dlIndex = paraList[i + 1];
                canBeSelected = bool.Parse(paraList[i + 2]);
                ChoiceItem choiceItem = new ChoiceItem(mark, dlIndex, canBeSelected, number++);
                choiceItemList.Add(choiceItem);
            }
            StageRenderManager.I.ChoiceCreate(choiceItemList);
        }

    }
}