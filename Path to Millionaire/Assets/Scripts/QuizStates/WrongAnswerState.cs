using UnityEngine;
using UnityEngine.UI;

public class WrongAnswerState : BaseState
{
    private int currentSelectedIndex;
    public WrongAnswerState(QuizManager quizManager, StateMachine stateMachine) : base(quizManager, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        quizManager.raycastoff.raycastTarget = true;
        quizManager.optionsButtons[currentSelectedIndex].GetComponent<Image>().color = quizManager.wrongAnswerColor;
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
    }

    public void SetCurrentSelectedIndex(int index)
    {
        currentSelectedIndex = index;
    }

}
