using UnityEngine;

public abstract class BaseState
{
    protected QuizManager quizManager;
    protected StateMachine stateMachine;

    protected bool isExitingState;

    protected float startTime;
    protected float elapsedTime;

    public BaseState(QuizManager quizManager, StateMachine stateMachine)
    {
        this.quizManager = quizManager;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        elapsedTime = 0f;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        isExitingState = true;
    }

    public virtual void LogicUpdate() { elapsedTime = Time.time - startTime; }

}
