using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private Vector2Int coordinate;
    private char character;
    private Color main = Color.white, highlight = Color.yellow;
    [SerializeField] private GameObject background, display;
    
    public void Background(Color color)
    {
        main = color;
        background.GetComponent<Image>().color = color;
    }
    public Color Background()
    {
        return main;
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
    public Color Highlight()
    {
        return highlight;
    }
    public void Coordinate(Vector2Int value)
    {
        coordinate = value;
    }
    public Vector2Int Coordinate()
    {
        return coordinate;
    }
    public void Character(char value)
    {
        character = value;
        display.GetComponent<TMP_Text>().text = value.ToString();
    }
    public char Character()
    {
        return character;
    }
}
