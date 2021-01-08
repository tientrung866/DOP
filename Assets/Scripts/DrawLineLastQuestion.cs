using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DrawLineLastQuestion : MonoBehaviour
{
    public GameObject questionImage;
    public GameObject answerImage;
    
    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private BoxCollider2D winBox;

    private LineRenderer lineRenderer;
    
    private bool gameHasEnded = false;
    [SerializeField] private float restartDelay = 2f;

    private GameObject questImg;
    
    private List<Vector2> points = new List<Vector2>();
    
    private bool correct = true;
    
    // Start is called before the first frame update
    void Start()
    {
        questImg = Instantiate(questionImage, Vector3.zero, Quaternion.identity);
        
        winBox = questImg.GetComponentInChildren<BoxCollider2D>();
        Debug.Log("winBox size" + winBox.size);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            lineRenderer = Instantiate(linePrefab);
            AddPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            if (Vector2.Distance(tempFingerPos, lineRenderer.GetPosition(lineRenderer.positionCount - 1)) > .1f)
            {
                AddPoint(tempFingerPos);
                points.Add(tempFingerPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            
            Debug.Log("number of points: " + points.Count);
            
            foreach (Vector2 item in points)
            {
                if (ToBeOutside(winBox,item))
                {
                    Debug.Log("winBox size" + winBox.size);
                    correct = false;
                    break;
                }
            }

            if (correct)
            {
                //thang
                Debug.Log("correct");
                //load answer
                Destroy(questImg);
                Instantiate(answerImage, Vector3.zero, Quaternion.identity);
                //load next
                NextQuestion();
            }
            
            if (!correct)
            {
                //thua
                Debug.Log("incorrect");
                //reload scence
                EndGame();
            }

            lineRenderer.positionCount = 0;
        }
    }
    
    void AddPoint(Vector2 newFingerPos)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
    }

    static public bool ToBeOutside(BoxCollider2D test, Vector2 point)
    {
        bool inside;
        inside = test.bounds.Contains(point);
        return !inside;
    }

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("INCORRECT!");
            Invoke("Restart", restartDelay);
        }
    }

    public void NextQuestion()
    {
        if (gameHasEnded == false)
        {
            Debug.Log("CORRECT!");
            Invoke("LoadNew", restartDelay);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene("Question02");
    }

    private void LoadNew()
    {
        SceneManager.LoadScene("z.CompleteScene");
    }
    
}
