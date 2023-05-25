using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private TMP_Text _num;
    public Vector2Int index;
    
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
