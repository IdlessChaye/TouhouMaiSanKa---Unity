using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class StageRenderManager : MonoBehaviour{
        #region Instance
        private static StageRenderManager instance;
        public static StageRenderManager I => instance;
        private void Awake() {
            if (instance == null) { 
                instance = this;
            } else {
                Destroy(this);
            }
            Initialize();
        }
        #endregion

        private PachiGrimoire pachiGrimoire;

        private void Initialize() {
            pachiGrimoire = PachiGrimoire.I;
        }
    }
}