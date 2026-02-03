using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private Vector2Int coordinate;
    private char character;
    private Color main = Color.white, highlight = Color.yellow;
    [SerializeField] private GameObject background, display;

    public void SetCoordinate(Vector2Int value)
    {
        coordinate = value;
    }
    public void SetCharacter(char value)
    {
        character = value;
        display.GetComponent<TMP_Text>().text = value.ToString();
    }
    public void SetColor(Color color)
    {

    }
    public void Highlight(bool value)
    {
        if (value)
        {
            background.GetComponent<Image>().color = highlight;
        }
        else
        {
            background.GetComponent<Image>().color = main;
        }
    }
    public Vector2Int GetCoordinate()
    {
        return coordinate;
    }
    public char GetCharacter()
    {
        return character;
    }
}
