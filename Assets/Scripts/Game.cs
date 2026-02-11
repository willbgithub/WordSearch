using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject CellPrefab, CellBoard;
    private const int COLUMNS=8, ROWS=8;
    [SerializeField] private float cellSize;
    private Cell[,] cells = new Cell[COLUMNS, ROWS];
    private List<Cell> unoccupiedCells;
    [SerializeField] private List<string> words;
    void Start()
    {
        GenerateBoard();
        AddWord("gum");
    }

    private void GenerateBoard()
    {
        CellBoard.GetComponent<GridLayoutGroup>().constraintCount = COLUMNS;
        CellBoard.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellSize, cellSize);
        for (int row = 0; row < COLUMNS; row++)
        {
            for (int column = 0; column < ROWS; column++)
            {
                Cell cell = Instantiate(CellPrefab, CellBoard.transform).GetComponent<Cell>();
                cells[column, row] = cell;
                unoccupiedCells.Add(cell);
                cell.Locked(false);
                cell.GetComponent<Transform>().localScale = new Vector3(cellSize/100, cellSize/100, 1);
            }
        }
    }

    private void AddWord(string word)
    {
        List<Cell> potentialCells = new List<Cell>();
        for (int row = 0; row < COLUMNS; row++)
        {
            for (int col = 0; col < ROWS; col++)
            {
                potentialCells.Add(cells[col, row]);
            }
        }
        bool completed = false;
        bool blacklist = false;
        int progress = 0;
        List<Cell> whitelist = new List<Cell>();
        // Pick a random cell and see if it works
        while(!blacklist && !completed && potentialCells.Count > 0)
        {
            // when locking all cells in whitelist, don't forget to lock originCell too!!!
            List<Vector2Int> directions = new List<Vector2Int>();
            directions.Add(new Vector2Int(0, -1)); // N
            directions.Add(new Vector2Int(1, 0)); // E
            directions.Add(new Vector2Int(0, 1)); // S
            directions.Add(new Vector2Int(-1, 0)); // W
            directions.Add(new Vector2Int(1, -1)); // NE
            directions.Add(new Vector2Int(-1, -1)); // NW
            directions.Add(new Vector2Int(1, 1)); // SE
            directions.Add(new Vector2Int(-1, 1)); // SW
            Cell originCell = potentialCells[Random.Range(0, potentialCells.Count-1)];
            Cell selectedCell = potentialCells[Random.Range(0, potentialCells.Count - 1)];
            potentialCells.Remove(originCell);
            // If the chosen cell already has a letter that doesn't line up, then abandon
            if (originCell.Locked() && originCell.Character() == word[0])
                blacklist = true;
            // See if the chosen cell works by expanding in each direction until it completes or all directions run out
            while (!blacklist && !completed && directions.Count > 0)
            {
                // Choose a random direction
                Vector2Int direction = directions[Random.Range(0, directions.Count-1)];
                directions.Remove(direction);
                // Expand in direction until completion or it fails
                while(!blacklist && !completed && progress < word.Length-1)
                {
                    // Cell is unlocked
                    if (!selectedCell.Locked())
                    {
                        selectedCell.Character(word[progress]);
                        whitelist.Add(selectedCell);
                        progress++;
                    }
                    // Cell is locked and has incorrect letter; failure
                    else if (selectedCell.Character() != word[progress])
                    {
                        whitelist.Clear();
                        blacklist = true;
                        progress = 0;
                    }
                    // Cell is locked, but has correct letter
                    else
                    {
                        progress++;
                    }
                    Vector2Int newCoordinate = new Vector2Int(selectedCell.Coordinate().x + direction.x, selectedCell.Coordinate().y + direction.y);
                    // Coordinate is out of bounds and there are still more letters to add; failure
                    if (!IsValidCoordinate(newCoordinate) && progress < word.Length-1)
                    {
                        whitelist.Clear();
                        blacklist = true;
                        progress = 0;
                    }
                    // Word is completed.
                    else if (progress == word.Length-1)
                    {
                        completed = true;
                    }
                    // Coordinate is not out of bounds and there are still more letters to add
                    else
                    {
                        selectedCell = cells[newCoordinate.x, newCoordinate.y];
                    }
                }
                blacklist = false;
            }
        }
        if (completed)
        {
            for (int i = 0; i < whitelist.Count; i++)
            {
                whitelist[i].Locked(true);
            }
        }
        else
        {
            print("Could not add '" + word + "' to board!");
        }
    }

    private bool IsValidCoordinate(Vector2Int coordinate)
    {
        return (coordinate.x >= 0 && coordinate.x < COLUMNS && coordinate.y >= 0 && coordinate.y < ROWS);
    }
}
