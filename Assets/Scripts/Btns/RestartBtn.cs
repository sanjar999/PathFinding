using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartBtn : MonoBehaviour
{
    void Start() => GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("Game"); });
}