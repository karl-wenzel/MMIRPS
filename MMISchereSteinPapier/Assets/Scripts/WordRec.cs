using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public class WordRec : MonoBehaviour
{
    // Define keywords (added cutter here as an extra for scissors)
    private readonly string[] keywords = {"stein", "papier", "schere", "nochmal", "rock", "paper", "scissors", "cutter", "again", "go", "neu" };

    // Using KeywordRecognizer
    private KeywordRecognizer keywordRecognizer;

    void Start()
    {
        // Initialize the KeywordRecognizer with the list of keywords.
        keywordRecognizer = new KeywordRecognizer(keywords);
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();

        // Introtext Terminal
        Debug.Log("Speech recognizer started. Please say 'rock', 'paper', or 'scissors'.");
    }

    void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Recognized: " + args.text);

        //Case 1: Go again button ToDo: Only make it an option when game is not running
        if (args.text.ToLower() == "go"||args.text.ToLower() == "nochmal"||args.text.ToLower() == "again"|| args.text.ToLower() == "neu")
        {
            GameManager.instance.ExecuteGameLoop();
        }
        // ToDo: Only make it an option when game is running
        else
        {
            // Case 2: in game
            // Convert the recognized text to an RPSOption.
            RPSOption option = ConvertKeywordToOption(args.text);

            // Choose  a button
            GameManager.instance.OnOptionChosen(option);
        }

    }

    RPSOption ConvertKeywordToOption(string keyword)
    {
        switch (keyword.ToLower())
        {
            case "rock":
                return RPSOption.Rock;
            case "stein":
            return RPSOption.Rock;
            case "paper":
                return RPSOption.Paper;
            case "papier":
                return RPSOption.Paper;
            case "schere":
                return RPSOption.Scissors;
            case "scissors":
                return RPSOption.Scissors;
            case "cutter":
                return RPSOption.Scissors;
            default:
                Debug.LogWarning("Unrecognized keyword: " + keyword);
                return default;
        }
    }

    private void OnDestroy()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            Debug.Log("Speech recognizer ended");
            keywordRecognizer.Stop();
            keywordRecognizer.OnPhraseRecognized -= OnPhraseRecognized;
        }
    }
}
