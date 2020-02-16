using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class MusicVoicePlayNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("VoicePlay");
            InterpretPart(context);
        }




        protected override void OnUpdateStageContext() {
            if (paraList.Count != 2)
                throw new System.Exception("MusicVoicePlayNode");
            string characterName = paraList[0]; // "SC_ScriptName"
            string voiceIndex = paraList[1];
            AudioClip clip = PachiGrimoire.I.ResourceManager.Get<AudioClip>(voiceIndex);

            StateBuff stateBuff = PachiGrimoire.I.StateMachine.StateBuff;
            if (stateBuff == StateBuff.Next) {
                PachiGrimoire.I.MusicManager.VoicePlay(characterName, clip, voiceIndex,false);
            } else {
                PachiGrimoire.I.MusicManager.VoicePlay(characterName, clip, voiceIndex);
            }
        }



    }
}