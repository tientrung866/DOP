using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DrawLine : MonoBehaviour
{
    public GameObject questionImage;
    public GameObject answerImage;
    public GameObject checkBoxes;
    
    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private BoxCollider2D winBox;

    [SerializeField] private CircleCollider2D[] checkBox;

    private LineRenderer lineRenderer;
    
    private bool gameHasEnded = false;
    [SerializeField] private float restartDelay = 2f;

    private GameObject questImg;
    private GameObject questImgCheck;
    
    private List<Vector2> points = new List<Vector2>();

    private bool inside = true;
    private bool allChecked = false;

    private int countCheck;
    
    void Start()
    {
        questImg = Instantiate(questionImage, Vector3.zero, Quaternion.identity);
        questImgCheck = Instantiate(checkBoxes, Vector3.zero, Quaternion.identity);
        
        winBox = questImg.GetComponentInChildren<BoxCollider2D>();
        checkBox = questImgCheck.GetComponentsInChildren<CircleCollider2D>();

        Debug.Log("winBox size" + winBox.size);
        Debug.Log("Checkboxes length: " + checkBox.Length);
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
            countCheck = 0;
            
            foreach (Vector2 item in points)
            {
                if (ToBeOutside(winBox,item))
                {
                    Debug.Log("winBox size" + winBox.size);
                    inside = false;
                    break;
                }
            }

            for (int j=0; j < points.Count; j++)
            {
                for (int i = 0; i < checkBox.Length; i++)
                {
                    if ( ToBeInside(checkBox[i],points[j]) && ToBeInside(checkBox[checkBox.Length - 1 - i],points[points.Count - 1 - j]) )
                    {
                        Debug.Log("ALL CHECKED!");
                        allChecked = true;
                        break;
                    }
                }
            }
            
            foreach (Vector2 item in points)
            {
                //countCheck = 0;
                for (int i = 0; i < checkBox.Length; i++)
                {
                    if (ToBeInside(checkBox[i], item))
                    {
                        countCheck++;
                        continue;
                    }
                }
                Debug.Log("Count checked: " + countCheck);
            }

            if (inside && allChecked && (countCheck >= checkBox.Length))
            {
                Debug.Log("correct");
                //load answer
                Destroy(questImg);
                Destroy(questImgCheck);
                Instantiate(answerImage, Vector3.zero, Quaternion.identity);
                
                NextQuestion();
            }

            else
            {
                Debug.Log("incorrect");
                
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

    static public bool ToBeInside(CircleCollider2D test, Vector2 point)
    {
        bool inside;
        inside = test.bounds.Contains(point);
        return inside;
    }

    static public bool ToBeOutside(BoxCollider2D test, Vector2 point)
    {
        bool inside;
        inside = test.bounds.Contains(point);
        return !inside;
    }

    private float BoundsContainedPercentage( Bounds obj, Bounds region )
    {
        var total = 1f;
 
        for ( var i = 0; i < 3; i++ )
        {
            var dist = obj.min[i] > region.center[i] ?
                obj.max[i] - region.max[i] :
                region.min[i] - obj.min[i];
 
            total *= Mathf.Clamp01(1f - dist / obj.size[i]);
        }
 
        return total;
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
        SceneManager.LoadScene("Question01");
    }

    private void LoadNew()
    {
        SceneManager.LoadScene("Question01Q1");
    }
    
}
