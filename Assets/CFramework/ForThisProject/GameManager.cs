using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Framework.MonoSingletons<GameManager>
{
    public class RegisterContent
    {
        public GridBehavior grid { get; set; }
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
