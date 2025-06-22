using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    public RPSOption option;
    private Button button;
    private Image image;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        GameManager.instance.OnOptionChosen(option);
    }

    public void SetChosenState(bool chosen)
    {
        image.color = chosen ? Color.green : Color.white;
    }
}