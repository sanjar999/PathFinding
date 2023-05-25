using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private GridGenerator _gridGenerator;
    [SerializeField] private Button _button;
    [SerializeField] private float _minDistToExit = 0.16f;
    private Enter _enter;
    private Exit _exit;

    private Tile[,] _grid;
    private List<Tile> _path = new List<Tile>();

    private Tile _currTile;
    private Tile _potentianNewTile;
    private float _distance = float.MaxValue;

    void Start()
    {
        _button.onClick.AddListener(Find);
        _enter = _gridGenerator.GetEnter;
        _exit = _gridGenerator.GetExit;
        _grid = _gridGenerator.GetGrid;

        GetFirstTile();
    }

    private void GetFirstTile()
    {
        if (_enter.index.x == -1)
        {
            _currTile = _grid[0, _enter.index.y];
            _currTile.index = new Vector2Int(0, _enter.index.y);
        }
        else
        {
            _currTile = _grid[9, _enter.index.y];
            _currTile.index = new Vector2Int(9, _enter.index.y);
        }

        _currTile.SetColor(Color.yellow);
        _path.Add(_currTile);
    }

    private void Find()
    {
        while (_distance> _minDistToExit)
        {
            CheckUp();
            CheckDown();
            CheckRight();
            CheckLeft();

            _currTile = _potentianNewTile;
            _currTile.SetColor(Color.yellow);
            _path.Add(_currTile);
        } 

    }

    #region  Checks

    private void CheckUp()
    {
        if (_currTile.index.y == 9)
            return;
        
        CheckSide(0,1);
    }

    private void CheckDown()
    {
        if (_currTile.index.y == 0)
            return;
        
        CheckSide(0,-1);
    }

    private void CheckRight()
    {
        if (_currTile.index.x == 9)
            return;
        
        CheckSide(1,0);
    }

    private void CheckLeft()
    {
        if (_currTile.index.x == 0)
            return;

        CheckSide(-1,0);
    }

    private void CheckSide(int x, int y)
    {
        var newTile = _grid[_currTile.index.x + x, _currTile.index.y + y];
        var newDist = Vector3.Distance(newTile.transform.position, _exit.transform.position);
        if (newDist < _distance)
        {
            _potentianNewTile = newTile;
            _potentianNewTile.index = new Vector2Int(_currTile.index.x + x, _currTile.index.y + y);
            _distance = newDist;
        }
    }
    
    #endregion

}