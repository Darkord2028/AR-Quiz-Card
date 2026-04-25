using System.IO;
using UnityEngine;

public class LoadState : BaseState
{
    public LoadState(QuizManager quizManager, StateMachine stateMachine) : base(quizManager, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        quizManager.ToggleBackground(true);
        quizManager.UpdateStarCount(0);
    }

    public override void Exit()
    {
        base.Exit();
        quizManager.ToggleBackground(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
