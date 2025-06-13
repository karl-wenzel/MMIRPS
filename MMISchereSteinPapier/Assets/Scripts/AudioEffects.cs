using UnityEngine;

public class AudioEffects : MonoBehaviour
{
    public static AudioEffects instance;
    public AudioSource src;
    //List of all clips used
    public AudioClip scissors, paper, rock, go_again, you_loose, you_win, its_a_draw, opponent_scissors, opponent_paper, opponent_rock, intro;

    private void Awake()
    {
        instance = this;
    }
    //Scissor/Rock/Paper Sound
    public void PlayOption(RPSOption option)
    {
        if (option == RPSOption.Scissors) PlayScissors();
        else if (option == RPSOption.Rock) PlayRock();
        else if (option == RPSOption.Paper) PlayPaper();
    }
    public void PlayScissors()
    {
        src.clip = scissors;
        src.Play();
    }
    public void PlayRock()
    {
        src.clip = rock;
        src.Play();
    }
    public void PlayPaper()
    {
        src.clip = paper;
        src.Play();
    }

    //Scissor/Rock/Paper Opponent Sound
    public void PlayOpponentOption(RPSOption option)
    {
        if (option == RPSOption.Scissors) PlayOpponentScissors();
        else if (option == RPSOption.Rock) PlayOpponentRock();
        else if (option == RPSOption.Paper) PlayOpponentPaper();
    }
    public void PlayOpponentScissors()
    {
        src.clip = opponent_scissors;
        src.Play();
    }
    public void PlayOpponentRock()
    {
        src.clip = opponent_rock;
        src.Play();
    }
    public void PlayOpponentPaper()
    {
        src.clip = opponent_paper;
        src.Play();
    }

    //Start Game sound
    public void PlayIntro()
    {
        src.clip = intro;
        src.Play();
    }

    //Restart Game sound
    public void PlayGoAgain()
    {
        src.clip = go_again;
        src.Play();
    }
    //Result sounds
    public void PlayWin()
    {
        src.clip = you_win;
        src.Play();
    }
    public void PlayLoose()
    {
        src.clip = you_loose;
        src.Play();
    }
        public void PlayDraw()
    {
        src.clip = its_a_draw;
        src.Play();
    }
}
