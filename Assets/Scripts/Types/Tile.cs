using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TileType{empty, path, obstacle, no_go}

public class Tile : MonoBehaviour
{
    [SerializeField] private TMP_Text _num;
    public Vector2Int index;
    public float _distToexit;
    
    private TileType _type;

    public TileType Type
    {
        get
        {
            return _type;
        }
        set
        {
            switch (value)
            {
                case TileType.empty:
                    SetColor(Color.white);
                    break;
                case TileType.path:
                    SetColor(Color.yellow);
                    break;
                case TileType.obstacle:
                    SetColor(Color.grey);
                    break;
                case TileType.no_go:
                    SetColor(Color.red);
                    break;
            }

            _type = value;
        }
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public void Clear()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void SetNum(int val)
    {
        _num.text = val.ToString();
    }
}
