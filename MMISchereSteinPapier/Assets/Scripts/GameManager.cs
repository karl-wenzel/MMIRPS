using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject ChoiceButtons;
    public GameObject OpponentPicksArea;
    public Image OwnChoiceImg;
    public OpponentSlotMachine OpponentSlotMachine;
    public TMP_Text Explaination;
    public TMP_Text WinLoose;
    public Button GoAgain;

    public Dictionary<RPSOption, RPSOption[]> Beats = new()
    {
        { RPSOption.Paper, new[] { RPSOption.Rock } },
        { RPSOption.Rock, new[] { RPSOption.Scissors } },
        { RPSOption.Scissors, new[] { RPSOption.Paper } }
    };

    private void Awake()
    {
        instance = this;
        GoAgain.onClick.AddListener(OnGoAgainClicked);
    }

    private void Start()
    {
        ExecuteGameLoop();
        AudioEffects.instance.PlayIntro();
    }

    public void ExecuteGameLoop()
    {
        ChoiceButtons.SetActive(true);
        OpponentPicksArea.SetActive(false);
        GoAgain.gameObject.SetActive(false);
        Explaination.text = string.Empty;
        WinLoose.text = string.Empty;
    }

    public async void OnOptionChosen(RPSOption option)
    {
        OwnChoiceImg.sprite = GraphicsManager.instance.GetOption(option);
        ChoiceButtons.SetActive(false);
        AudioEffects.instance.PlayOption(option);
        OpponentPicksArea.SetActive(true);
        
        var opponentChoice = (RPSOption)Random.Range(0, System.Enum.GetValues(typeof(RPSOption)).Length);
        
        OpponentSlotMachine.SetOption(opponentChoice);
        await Awaitable.WaitForSecondsAsync(1.2f);
        AudioEffects.instance.PlayOpponentOption(opponentChoice);
        await Awaitable.WaitForSecondsAsync(1f);

        var gameStatus = GameStatus.Draw;
        if (Beats[option].Contains(opponentChoice))
        {
            gameStatus = GameStatus.Win;
            Explaination.text = $"{option} beats {opponentChoice}";
        }
        else if (Beats[opponentChoice].Contains(option))
        {
            gameStatus = GameStatus.Loose;
            Explaination.text = $"{opponentChoice} beats {option}";
        }
        else
        {
            gameStatus = GameStatus.Draw;
            Explaination.text = $"Great minds think alike";
        }

        await Awaitable.WaitForSecondsAsync(0.6f);

        switch (gameStatus)
        {
            case GameStatus.Draw:
                WinLoose.text = "Noone wins.";
                AudioEffects.instance.PlayDraw();
                break;
            case GameStatus.Win:
                WinLoose.text = "You win!";
                AudioEffects.instance.PlayWin();
                break;
            case GameStatus.Loose:
                WinLoose.text = "You lost.";
                AudioEffects.instance.PlayLoose();
                break;
        }

        await Awaitable.WaitForSecondsAsync(0.6f);
        GoAgain.gameObject.SetActive(true);
    }

    public void OnGoAgainClicked()
    {
        AudioEffects.instance.PlayGoAgain();
        ExecuteGameLoop();
    }

    enum GameStatus
    {
        Draw,
        Win,
        Loose
    }
}
