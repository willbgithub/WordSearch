using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject CellPrefab, CellBoard;
    [SerializeField] private int sizeX, sizeY;
    [SerializeField] private float cellSize;
    void Start()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        CellBoard.GetComponent<GridLayoutGroup>().constraintCount = sizeX;
        CellBoard.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellSize, cellSize);
        for (int row = 0; row < sizeX; row++)
        {
            for (int col = 0; col < sizeY; col++)
            {
                Cell cell = Instantiate(CellPrefab, CellBoard.transform).GetComponent<Cell>();
                cell.GetComponent<Transform>().localScale = new Vector3(cellSize/100, cellSize/100, 1);
            }
        }
    }
}
