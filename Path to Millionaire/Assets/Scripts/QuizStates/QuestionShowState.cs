using UnityEngine;

public class QuestionShowState : BaseState
{
    public QuestionShowState(QuizManager quizManager, StateMachine stateMachine) : base(quizManager, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (quizManager.currentQuestionIndex > quizManager.questionData.questions.Count - 1)
        {
            quizManager.UIAnimations.Cut(true);
            quizManager.QuizFinishImage.SetActive(true);
        }
        else
        {
            quizManager.UIAnimations.Cut(false);
            LeanTween.delayedCall(quizManager.UIAnimations.tweenTime, Init);
        }
    }

    private void Init()
    {
        quizManager.ShowQuestion();
        quizManager.currentQuestionIndex++;
        stateMachine.ChangeState(quizManager.checkAnswerState);

        foreach (var button in quizManager.optionsButtons)
        {
            button.interactable = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
