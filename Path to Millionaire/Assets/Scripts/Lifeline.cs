using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public enum LifelineType
{
    FiftyFifty,
    Skip,
    AudiencePoll
}
public class Lifeline : MonoBehaviour
{
    public QuizManager quizManager;
    private Dictionary<LifelineType, bool> lifelineUsed = new Dictionary<LifelineType, bool>();
    private void Start()
    {
        foreach (LifelineType type in System.Enum.GetValues(typeof(LifelineType)))
        {
            lifelineUsed[type] = false;
        }
    }

    [Button("50-50")]
    public void UseFiftyFifty()
    {
        if (lifelineUsed[LifelineType.FiftyFifty] == false)
        {
            lifelineUsed[LifelineType.FiftyFifty] = true;
            quizManager.checkAnswerState.shouldTick = false;
            quizManager.raycastoff.raycastTarget = true;
            AudioManager.Instance.Play(AudioManager.Instance.fiftyfifty);
            Debug.Log("50-50 Lifeline used!");
            LeanTween.delayedCall(AudioManager.Instance.audioEndTime, RemoveOption);
        }
        else
        {
            Debug.Log("50-50 Used");
        }
    }
    public void RemoveOption()
    {
        List<int> wrongIndexes = new List<int>();

        // Get options for current question
        var options = quizManager.questionData.questions[quizManager.currentQuestionIndex - 1].options;

        // Collect ONLY wrong option indices
        for (int i = 0; i < options.Count; i++)
        {
            if (!options[i].isCorrect)
                wrongIndexes.Add(i);
        }

        // Disable exactly 2 wrong answers if possible
        int toDisable = Mathf.Min(2, wrongIndexes.Count);
        for (int n = 0; n < toDisable; n++)
        {
            int rand = UnityEngine.Random.Range(0, wrongIndexes.Count);
            int indexToDisable = wrongIndexes[rand];

            // Safety: Ensure we never disable the correct one
            if (options[indexToDisable].isCorrect == false)
            {
                Debug.Log(quizManager.optionsButtons[indexToDisable]);
                quizManager.optionsButtons[indexToDisable].interactable = false;
            }

            wrongIndexes.RemoveAt(rand);
        }
        quizManager.raycastoff.raycastTarget = false;
        quizManager.checkAnswerState.shouldTick = true;

    }

    [Button("Skip")]
    public void UseSkip()
    {
        if (lifelineUsed[LifelineType.Skip] == false)
        {
            quizManager.stateMachine.ChangeState(quizManager.correctAnswerState);
            lifelineUsed[LifelineType.Skip] = true;
            Debug.Log("Skip Lifeline used!");
            AudioManager.Instance.Play(AudioManager.Instance.SkipQues);
        }
        else
        {
            Debug.Log("Skip Used");
        }
    }

    [Button("Audience Poll")]
    public void UseAudiencePoll()
    {
        if (lifelineUsed[LifelineType.AudiencePoll] == false)
        {
            quizManager.stateMachine.ChangeState(quizManager.correctAnswerState);
            lifelineUsed[LifelineType.AudiencePoll] = true;
            Debug.Log("Audience Poll Lifeline used!");
            AudioManager.Instance.Play(AudioManager.Instance.Audience);
        }
        else
        {
            Debug.Log("Audience Poll Used");
        }
    }
}
