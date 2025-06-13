using UnityEngine;

public class AudioEffects : MonoBehaviour
{
    public AudioSource src;
    public AudioClip scissors, paper, rock, go_again;

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
    public void PlayGoAgain() {
        src.clip = go_again;
        src.Play();
    } 
}
