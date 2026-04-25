using System;
using UnityEngine;
using UnityEngine.UI;

public class CheckAnswerState : BaseState
{
    private int currentQuestionTime;
    private int lastSecond;
    public bool shouldTick = true;

    public CheckAnswerState(QuizManager quizManager, StateMachine stateMachine) : base(quizManager, stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        quizManager.raycastoff.raycastTarget = false;
        shouldTick = true;
        quizManager.OnCorrectAnswerEvent += OnCorrectAnswer;
        quizManager.OnWrongAnswerEvent += OnWrongAnswer;

        currentQuestionTime = quizManager.questionTimeInSeconds;
        lastSecond = 0;

        for (int i = 0; i < quizManager.optionsButtons.Count; i++)
        {
            quizManager.optionsButtons[i].GetComponent<Image>().color = quizManager.defaultColor;
        }

        quizManager.ShowTimer(currentQuestionTime);
    }

    private void OnCorrectAnswer(int index)
    {
        quizManager.raycastoff.raycastTarget = true;
        AudioManager.Instance.Stop();
        shouldTick = false;
        Debug.Log("Correct answer!");      
        quizManager.optionsButtons[index].GetComponent<Image>().color = quizManager.selectedColor;
        AudioManager.Instance.Play(AudioManager.Instance.lockOption);
        LeanTween.delayedCall(AudioManager.Instance.audioEndTime, TransitionToCorrectState);
    }

    private void OnWrongAnswer(int index)
    {
        quizManager.raycastoff.raycastTarget = true;
        AudioManager.Instance.Stop();
        shouldTick = false;
        Debug.Log("Wrong answer!");
        quizManager.optionsButtons[index].GetComponent<Image>().color = quizManager.selectedColor;
        AudioManager.Instance.Play(AudioManager.Instance.lockOption);
        LeanTween.delayedCall(AudioManager.Instance.audioEndTime, TransitionToWrongState);
        quizManager.wrongAnswerState.SetCurrentSelectedIndex(index);
    }

    private void TransitionToCorrectState()
    {
        AudioManager.Instance.Play(AudioManager.Instance.clickRightAns);
        AudioManager.Instance.Play(AudioManager.Instance.selectedRightAns);
        quizManager.stateMachine.ChangeState(quizManager.correctAnswerState);
    }

    private void TransitionToWrongState()
    {
        AudioManager.Instance.Play(AudioManager.Instance.clickWrongAns);
        AudioManager.Instance.Play(AudioManager.Instance.selectedWrongAns);
        quizManager.stateMachine.ChangeState(quizManager.wrongAnswerState);

    }

    public override void Exit()
    {
        base.Exit();
        shouldTick = false;
        quizManager.OnCorrectAnswerEvent -= OnCorrectAnswer;
        quizManager.OnWrongAnswerEvent -= OnWrongAnswer;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        int secondsPassed = Mathf.FloorToInt(elapsedTime);

        if (secondsPassed > lastSecond && shouldTick)
        {
            if (currentQuestionTime == 0)
            {
                AudioManager.Instance.Stop();
                shouldTick = false;
                Debug.Log("Time's Up!");
                OnWrongAnswer(0);
                return;
            }

            int delta = secondsPassed - lastSecond;
            currentQuestionTime -= delta;
            lastSecond = secondsPassed;

            quizManager.ShowTimer(currentQuestionTime);
            float normalizedTime = (float)lastSecond / (float)quizManager.questionTimeInSeconds;
            quizManager.UpdateTimerFill(normalizedTime);
        }

    }
}
