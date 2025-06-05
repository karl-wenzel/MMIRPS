using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class OpponentSlotMachine : MonoBehaviour
{
    public Animator Animator;
    public GameObject AnimatedArea;
    public Image SelectedOption;
    public Image[] otherOptions;

    void Awake()
    {
        AnimatedArea.SetActive(false);
    }

    public void SetOption(RPSOption option)
    {
        foreach (var img in otherOptions)
        {
            img.sprite = GraphicsManager.instance.GetRandomOption();
        }
        SelectedOption.sprite = GraphicsManager.instance.GetOption(option);
        Animator.SetTrigger("PlaySlotAnimation");
        AnimatedArea.SetActive(true);
    }

    private void OnDisable()
    {
        AnimatedArea?.SetActive(false);
    }
}
