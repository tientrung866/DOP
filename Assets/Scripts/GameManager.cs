using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject gameStartCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /*public void GameOver()
    {
        gameStartCanvas.SetActive(true);
        Time.timeScale = 0;
    }*/

    public void GameStartOrReplay()
    {
        SceneManager.LoadScene("Question01");
    }
    
}
