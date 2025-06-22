using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject GameExplanation;
    public GameObject ChoiceButtonArea;
    public GameObject OpponentPicksArea;
    public Image OwnChoiceImg;
    public OpponentSlotMachine OpponentSlotMachine;
    public TMP_Text Explaination;
    public TMP_Text WinLoose;
    public Button GoAgain;
    public Slider TimeSlider;

    private ChoiceButton[] ChoiceButtons;

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
        ChoiceButtons = FindObjectsByType<ChoiceButton>(FindObjectsSortMode.None);
    }

    private void Start()
    {
        ChoiceButtonArea.SetActive(false);
        OpponentPicksArea.SetActive(false);
        GoAgain.gameObject.SetActive(true);
        GameExplanation.SetActive(true);
        Explaination.gameObject.SetActive(true);
        WinLoose.gameObject.SetActive(true);
        TimeSlider.gameObject.SetActive(false);
        Explaination.text = string.Empty;
        WinLoose.text = string.Empty;
        AudioEffects.instance.PlayIntro();
    }

    bool executingGameLoop = false;

    public void ExecuteGameLoop()
    {
        executingGameLoop = true;
        ChoiceButtonArea.SetActive(true);
        GameExplanation.SetActive(false);
        OpponentPicksArea.SetActive(false);
        GoAgain.gameObject.SetActive(false);
        TimeSlider.gameObject.SetActive(true);
        Explaination.text = string.Empty;
        WinLoose.text = string.Empty;

        lastChosenOption = null;

        foreach (var b in ChoiceButtons)
        {
            b.SetChosenState(false);
        }

        StartCoroutine(ManageTimer());
    }

    RPSOption? lastChosenOption;

    IEnumerator ManageTimer()
    {
        const float timerMax = 5f;
        float timer = timerMax;
        while (timer > 0)
        {
            TimeSlider.value = (timerMax - timer) / timerMax + 0.1f;
            timer -= Time.deltaTime;
            yield return null;
        }

        AfterTimer();
    }

    public void OnOptionChosen(RPSOption option)
    {
        lastChosenOption = option;

        foreach (var b in ChoiceButtons)
        {
            b.SetChosenState(b.option == option);
        }
    }

    public async void AfterTimer()
    {
        TimeSlider.gameObject.SetActive(false);
        var option = lastChosenOption;
        OwnChoiceImg.sprite = option == null ? null : GraphicsManager.instance.GetOption(option.Value);
        ChoiceButtonArea.SetActive(false);
        if (option == null)
        {
            AudioEffects.instance.PlayNothing();
        }
        else
        {
            AudioEffects.instance.PlayOption(option.Value);
        }
        OpponentPicksArea.SetActive(true);

        var opponentChoice = (RPSOption)Random.Range(0, System.Enum.GetValues(typeof(RPSOption)).Length);

        OpponentSlotMachine.SetOption(opponentChoice);
        await Awaitable.WaitForSecondsAsync(1.2f);
        AudioEffects.instance.PlayOpponentOption(opponentChoice);
        await Awaitable.WaitForSecondsAsync(1f);

        var gameStatus = GameStatus.Draw;
        if (option == null)
        {
            gameStatus = GameStatus.Loose;
            Explaination.text = "You lost, because you didn't choose something.";
        }
        else if (Beats[option.Value].Contains(opponentChoice))
        {
            gameStatus = GameStatus.Win;
            Explaination.text = $"{option} beats {opponentChoice}";
        }
        else if (Beats[opponentChoice].Contains(option.Value))
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
        executingGameLoop = false;

        await Awaitable.WaitForSecondsAsync(0.6f);
        if (!executingGameLoop)
        {
            GoAgain.gameObject.SetActive(true);
        }
    }

    public void OnGoAgainClicked()
    {
        if (!executingGameLoop)
        {
            AudioEffects.instance.PlayGoAgain();
            ExecuteGameLoop();
        }
    }

    enum GameStatus
    {
        Draw,
        Win,
        Loose
    }
}
