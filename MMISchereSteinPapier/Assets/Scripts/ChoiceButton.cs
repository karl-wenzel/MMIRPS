using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    public RPSOption option;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        GameManager.instance.OnOptionChosen(option);
    }
}