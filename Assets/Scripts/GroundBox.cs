using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundBox : MonoBehaviour
{
    [SerializeField] private float restartDelay = 2f;
    [SerializeField] private GameObject answerImg;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(answerImg);
        Invoke("Reload", restartDelay);
    }
    
    private void Reload()
    {
        SceneManager.LoadScene("1.HowToPlay");
    }

    public void GoBack()
    {
        SceneManager.LoadScene("0.Start");
    }
}
