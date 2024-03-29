using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElosBlock
{
    public class GameManager : Framework.MonoSingletons<GameManager>
    {
        public class RegisterContent
        {
            public CustomGrid grid { get; set; }
            public StageController stageController { get; set; }
        }

        public RegisterContent Register { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            SetDontDestroyOnLoad();
            Register = new RegisterContent();
        }
    }
}