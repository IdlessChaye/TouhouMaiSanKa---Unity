using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineChoiceCreateNode : FunNode {

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("ChoiceCreate");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() { //Engine_ChoiceCreate(myFirstMark,*DL_Choice_0*,true,<Engine_ScriptLoadFile(*SC_SomeScriptName*)>,mySecondMark,*DL_Choice_1*,false,<Engine_ScriptLoadFile(*SC_OtherScriptName*)>)
            if (paraList.Count == 0 || paraList.Count % 4 != 0)
                throw new System.Exception("EngineChoiceCreateNode");

            StateMachineManager stateMachine = PachiGrimoire.I.StateMachine;
            stateMachine.TransferStateTo(ChoiceWaitState.Instance);
            StateBuff stateBuff = stateMachine.StateBuff;
            if(stateBuff == StateBuff.Next) {
                stateMachine.SetStateBuff(StateBuff.Normal);
            }

            List<ChoiceItem> choiceItemList = new List<ChoiceItem>();
            string mark;
            string dlIndex;
            bool canBeSelected;
            string onSelectedScirptContext;
            for (int i = 0; i < paraList.Count; i += 4) {
                mark = paraList[i];
                dlIndex = paraList[i + 1];
                canBeSelected = bool.Parse(paraList[i + 2]);
                onSelectedScirptContext = paraList[i + 3];
                ChoiceItem choiceItem = new ChoiceItem(mark, dlIndex, canBeSelected, onSelectedScirptContext);
                choiceItemList.Add(choiceItem);
            }
            StageRenderManager.I.ChoiceCreate(choiceItemList);
        }

    }
}