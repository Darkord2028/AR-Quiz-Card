using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ResponseWrapper
{
    public string message;
}

public class APIConnect : MonoBehaviour
{
    private QuizManager quizManager;

    [SerializeField] bool useLocalJson = false;

    [Header("API Settings")]
    public string apiUrl;
    public string url;
    public int numberOfQuestions;

    private void Start()
    {
        quizManager = GetComponent<QuizManager>();

        if (!useLocalJson)
        {
            CallAPI();
        }
        else
        {
            LeanTween.delayedCall(2f, ReadLocalJson);
        }
    }

    private async Task UploadAndLoadQuestions(string pdfUrl, int num)
    {
        string postJson = $"{{ \"url\":\"{pdfUrl}\", \"number\":{num} }}";

        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl, postJson, "application/json"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            var operation = request.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + request.error);
                return;
            }

            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Raw API Response: " + jsonResponse);

            ResponseWrapper wrapper = JsonUtility.FromJson<ResponseWrapper>(jsonResponse);
            if (wrapper == null || string.IsNullOrEmpty(wrapper.message))
            {
                Debug.LogError("Invalid API response format.");
                return;
            }

            string innerJson = wrapper.message
                .Replace("```json", "")
                .Replace("```", "")
                .Replace("json\n", "")
                .Trim();

            if (innerJson.StartsWith("["))
                innerJson = "{ \"questions\": " + innerJson + " }";

            Debug.Log("JSON after: " + innerJson);

            try
            {
                quizManager.questionData = JsonUtility.FromJson<QuestionsData>(innerJson);

                if (quizManager.questionData?.questions == null || quizManager.questionData.questions.Count == 0)
                {
                    Debug.LogError("No questions parsed from API.");
                    return;
                }

                Debug.Log($"Loaded {quizManager.questionData.questions.Count} questions.");
                quizManager.ShuffleOptions();
                quizManager.stateMachine.ChangeState(quizManager.questionShowState);
            }
            catch (Exception e)
            {
                Debug.LogError("JSON Parsing Error: " + e.Message);
                CallAPI(); // retry on parsing failure
            }
        }
    }

    public async void CallAPI()
    {
        await UploadAndLoadQuestions(url, numberOfQuestions);
    }

    private void ReadLocalJson()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "questions.json");
        string rawJson = File.ReadAllText(path);

        // Wrap the array into an object if it starts with '['
        if (rawJson.TrimStart().StartsWith("["))
        {
            rawJson = "{ \"questions\": " + rawJson + " }";
        }

        quizManager.questionData = JsonUtility.FromJson<QuestionsData>(rawJson);

        Debug.Log("Loaded " + quizManager.questionData.questions.Count + " questions");
        quizManager.ShuffleOptions();
        quizManager.stateMachine.ChangeState(quizManager.questionShowState);
    }
}
