using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem;
using Framework;
using UnityEngine.Events;

namespace ElosBlock
{
    public class StageInputHandler : MonoBehaviour
    {
        private StageController mStageController;
        private Playing.TetrisController activeBlock => mStageController.ActiveBlock;
        private PlayerInput mPlayerInput;

        private void Start()
        {
            mPlayerInput = GetComponent<PlayerInput>();
            mPlayerInput.onActionTriggered += MapPlayerInput;

            mStageController = GetComponent<StageController>();

            mStageController.OnTouchGround.AddListenerWithCustomUnRegister(StopLoopCall).UnRegisterWhenGameObjectDestroyed(gameObject);
            //activeBlock.OnMoveFail.AddListener(StopLoopCall);
        }

        private void OnDestroy()
        {
            mPlayerInput.onActionTriggered -= MapPlayerInput;
        }

        private void StopLoopCall()
        {
            Framework.TimeAction.Instance.StopLoopCall("activeBlock.MoveLeft");
            Framework.TimeAction.Instance.StopLoopCall("activeBlock.MoveRight");
            Framework.TimeAction.Instance.StopLoopCall("activeBlock.MoveDown");
        }

        private void MapPlayerInput(InputAction.CallbackContext ctx)
        {
            switch (ctx.action.name)
            {
                case "MoveLeft":
                    OnMoveLeftInput(ctx); break;
                case "MoveRight":
                    OnMoveRightInput(ctx); break;
                case "MoveDown":
                    OnMoveDownInput(ctx); break;
                case "Rotate":
                    OnRotateInput(ctx); break;
                case "DropToGround":
                    OnDropToGroundInput(ctx); break;
            }
        }

        public void OnMoveLeftInput(InputAction.CallbackContext ctx)
        {
            switch (ctx.phase)
            {
                case InputActionPhase.Started:
                    activeBlock.MoveLeft();
                    break;
                case InputActionPhase.Performed:
                    if (ctx.interaction is HoldInteraction)
                        Framework.TimeAction.Instance.LoopCall("activeBlock.MoveLeft", activeBlock.MoveLeft, 0.1f);
                    break;
                case InputActionPhase.Canceled:
                    if (ctx.interaction is HoldInteraction)
                        Framework.TimeAction.Instance.StopLoopCall("activeBlock.MoveLeft");
                    break;
                default:
                    break;
            }
        }

        public void OnMoveRightInput(InputAction.CallbackContext ctx)
        {
            switch (ctx.phase)
            {
                case InputActionPhase.Started:
                    activeBlock.MoveRight();
                    break;
                case InputActionPhase.Performed:
                    if (ctx.interaction is HoldInteraction)
                        Framework.TimeAction.Instance.LoopCall("activeBlock.MoveRight", activeBlock.MoveRight, 0.1f);
                    break;
                case InputActionPhase.Canceled:
                    if (ctx.interaction is HoldInteraction)
                        Framework.TimeAction.Instance.StopLoopCall("activeBlock.MoveRight");
                    break;
                default:
                    break;
            }
        }

        public void OnMoveDownInput(InputAction.CallbackContext ctx)
        {
            switch (ctx.phase)
            {
                case InputActionPhase.Started:
                    activeBlock.MoveDown();
                    break;
                case InputActionPhase.Performed:
                    if (ctx.interaction is HoldInteraction)
                        Framework.TimeAction.Instance.LoopCall("activeBlock.MoveDown", activeBlock.MoveDown, 0.1f);
                    break;
                case InputActionPhase.Canceled:
                    if (ctx.interaction is HoldInteraction)
                        Framework.TimeAction.Instance.StopLoopCall("activeBlock.MoveDown");
                    break;
                default:
                    break;
            }
        }

        public void OnRotateInput(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Started)
                activeBlock.Rotate();
            
        }

        public void OnDropToGroundInput(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Started)
                activeBlock.DropToGround();
        }
    }
}