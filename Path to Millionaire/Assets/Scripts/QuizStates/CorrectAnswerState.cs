using UnityEngine;
using UnityEngine.UI;

public class CorrectAnswerState : BaseState
{
    private bool starUpdated = false;
    public CorrectAnswerState(QuizManager quizManager, StateMachine stateMachine) : base(quizManager, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        starUpdated = false;
        quizManager.raycastoff.raycastTarget = true;
        quizManager.optionsButtons[quizManager.currentAnswerIndex].GetComponent<Image>().color = quizManager.rightAnswerColor;
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (elapsedTime > 5)
        {
            quizManager.stateMachine.ChangeState(quizManager.questionShowState);
        }
        if (elapsedTime > 1f)
        {
            quizManager.correctAnswerParticles.Play();
        }
        if (!starUpdated && quizManager.correctAnswerParticles.isStopped)
        {
            quizManager.UpdateStarCount(1);
            starUpdated = true;
        }
    }

}
