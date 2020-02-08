using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class StageRenderManager {
        private StageContextManager contextManager;
        private BacklogManager backlogManager;
        public BacklogManager BacklogManager => backlogManager;

        public StageRenderManager(StageContextManager contextManager) {
            this.contextManager = contextManager;
            this.backlogManager = new BacklogManager();
        }
    }
}