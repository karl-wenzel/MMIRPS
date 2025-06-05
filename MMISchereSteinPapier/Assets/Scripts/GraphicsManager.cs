using System.Linq;
using UnityEngine;

public class GraphicsManager : MonoBehaviour
{
    public static GraphicsManager instance;

    public IconEntry[] icons;

    [System.Serializable]
    public struct IconEntry
    {
        public RPSOption option;
        public Sprite sprite;
    }

    private void Awake()
    {
        instance = this;
    }

    public Sprite GetRandomOption()
    {
        return icons[Random.Range(0, icons.Length)].sprite;
    }

    public Sprite GetOption(RPSOption option)
    {
        return icons.First(x => x.option == option).sprite;
    }
}
