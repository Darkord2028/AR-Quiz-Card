using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Option
{
    public string text;
    public bool isCorrect;
}

[System.Serializable]
public class Question
{
    public string _id;
    public string questionText;
    public string questionType;
    public List<Option> options;
}

[System.Serializable]
public class QuestionsData
{
    public List<Question> questions;
}

public class QuizManager : MonoBehaviour
{
    #region Quiz States

    public StateMachine stateMachine { get; private set; }
    public LoadState loadState { get; private set; }
    public QuestionShowState questionShowState { get; private set; }
    public CheckAnswerState checkAnswerState { get; private set; }
    public CorrectAnswerState correctAnswerState { get; private set; }
    public WrongAnswerState wrongAnswerState { get; private set; }

    #endregion

    public QuestionsData questionData;

    [Header("Quiz Reference")]
    [SerializeField] TextMeshProUGUI questionBlock;
    public List<Button> optionsButtons;
    [SerializeField] public List<TextMeshProUGUI> optionTexts;
    [SerializeField] TextMeshProUGUI countdownTimer;
    [SerializeField] Image countdownFill;
    [SerializeField] GameObject loadingBackground;
    public Image raycastoff;

    [Header("Quiz Config")]
    public int questionTimeInSeconds = 30;
    public Color defaultColor;
    public Color selectedColor;
    public Color rightAnswerColor;
    public Color wrongAnswerColor;


    [Header("UI Animation References")]
    public UIAnimations UIAnimations;
    public GameObject QuizFinishImage;
    public TextMeshProUGUI starsCountText;

    [Header("Particles")]
    public ParticleSystem correctAnswerParticles;

    public int currentQuestionIndex = 0;
    public int currentAnswerIndex;

    public int currentStars { get; private set; }

    public event OnCorrectAnswerDelegate OnCorrectAnswerEvent;
    public delegate void OnCorrectAnswerDelegate(int index);

    public event OnWrongAnswerDelegate OnWrongAnswerEvent;
    public delegate void OnWrongAnswerDelegate(int index);

    private void Awake()
    {
        stateMachine = new StateMachine();
        loadState = new LoadState(this, stateMachine);
        questionShowState = new QuestionShowState(this, stateMachine);
        checkAnswerState = new CheckAnswerState(this, stateMachine);
        correctAnswerState = new CorrectAnswerState(this, stateMachine);
        wrongAnswerState = new WrongAnswerState(this, stateMachine);

        for (int i = 0; i < optionsButtons.Count; i++)
        {
            int index = i;
            optionsButtons[i].onClick.AddListener(() => OnOptionClick(index));
            TextMeshProUGUI txt = optionsButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            optionTexts.Add(txt);
        }
    }

    void Start()
    {
        raycastoff.raycastTarget = false;
        stateMachine.InitializeState(loadState);
        currentStars = 0;
        AudioManager.Instance.Stop();
    }

    void Update()
    {
        stateMachine.CurrentState.LogicUpdate();
    }

    public void OnOptionClick(int optionIndex)
    {
        if (optionIndex == currentAnswerIndex)
        {
            OnCorrectAnswerEvent?.Invoke(optionIndex);
        }
        else
        {
            OnWrongAnswerEvent?.Invoke(optionIndex);
        }
    }

    public void ShowQuestion()
    {
        Question question = questionData.questions[currentQuestionIndex];
        questionBlock.text = question.questionText;

        for (int i = 0; i < optionTexts.Count; i++)
        {
            optionTexts[i].text = questionData.questions[currentQuestionIndex].options[i].text;
        }

        currentAnswerIndex = questionData.questions[currentQuestionIndex].options.FindIndex(option => option.isCorrect);
    }

    public void ShuffleOptions()
    {
        for (int i = 0; i < questionData.questions.Count; i++)
        {
            System.Random rng = new System.Random();
            int optionCount = questionData.questions[i].options.Count;
            while (optionCount > 1)
            {
                optionCount--;
                int k = rng.Next(optionCount + 1);
                Option value = questionData.questions[i].options[k];
                questionData.questions[i].options[k] = questionData.questions[i].options[optionCount];
                questionData.questions[i].options[optionCount] = value;
            }
        }
    }

    public void ShowTimer(int time)
    {
        countdownTimer.transform.parent.gameObject.SetActive(true);
        countdownFill.fillAmount = 1;

        int minutes = time / 60;
        int seconds = time % 60;

        countdownTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        countdownFill.fillAmount = 1;
        countdownFill.color = Color.green;
        UpdateTimerFill(time);
        AudioManager.Instance.Stop();
    }

    public void HideTimer()
    {
        countdownTimer.transform.parent.gameObject.SetActive(false);
        countdownTimer.text = "0";
        AudioManager.Instance.Stop();
    }

    public void UpdateTimerFill(float time)
    {
        countdownFill.fillAmount = 1 - time;
        if (countdownFill.fillAmount > 0.5f)
        {
            countdownFill.color = Color.green;
            AudioManager.Instance.Play(AudioManager.Instance.greenAudio);
        }
        else if (countdownFill.fillAmount <= 0.5f && countdownFill.fillAmount > 0.3f)
        {
            countdownFill.color = Color.yellow;
            AudioManager.Instance.Play(AudioManager.Instance.yellowAudio);
        }
        else if (countdownFill.fillAmount <= 0.3f)
        {
            countdownFill.color = Color.red;
            AudioManager.Instance.Play(AudioManager.Instance.redAudio);
        }
    }

    public void ToggleBackground(bool value)
    {
        loadingBackground.SetActive(value);
    }

    [ContextMenu("Show Question Data")]
    public void ShowQuestionData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "questions.json");
        string json = File.ReadAllText(path);

        QuestionsData questionData = JsonUtility.FromJson<QuestionsData>(json);
    }

    public void UpdateStarCount(int starsGained)
    {
        currentStars += starsGained;
        starsCountText.text = currentStars.ToString();
    }

}
