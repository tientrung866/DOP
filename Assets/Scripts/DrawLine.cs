using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DrawLine : MonoBehaviour
{
    /*[SerializeField] private GameObject questionImage;
    [SerializeField] private GameObject answerImage;*/
    
    public GameObject questionImage;
    public GameObject answerImage;
    
    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private BoxCollider2D winBox;
    //[SerializeField] private CircleCollider2D dotS;
    
    private LineRenderer lineRenderer;
    
    private bool gameHasEnded = false;
    [SerializeField] private float restartDelay = 2f;

    private GameObject questImg;
    
    //Vector2 tempFingerPoses = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    private List<Vector2> points = new List<Vector2>();
    
        /*if (Vector2.Distance(tempFingerPoses, lineRenderer.GetPosition(lineRenderer.positionCount - 1)) > .1f)
    {
        AddPoint(tempFingerPoses);
        points.Add(tempFingerPoses);
    }*/
    
    /*private EdgeCollider2D edgeColl;
    private List<Vector2> points;*/
    
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
            /*Debug.Log("points" + points);*/
        }

        if (Input.GetMouseButtonUp(0))
        {
            /*winBox = questImg.GetComponentInChildren<BoxCollider2D>();
            Debug.Log("winBox size" + winBox.size);*/
            //Debug.Log("winBox size" + winBox.size);

            /*phan thang thua*/
            /*foreach (Vector2 item in points)
            {
                if( !namtronghop )
                {
                    correct = false;
                    break;
                }
            }

            if (correct = 0)
            {
                // thang
            }
            else if (correct = 1)
            {
                // thua
            }*/
            
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
    
    /*public static bool IsInside(BoxCollider2D c, Vector2 point)
    {
        Vector2 closest = c.ClosestPoint(point);
        // Because closest=point if point is inside - not clear from docs I feel
        return closest == point;
    }*/

    static public bool ToBeOutside(BoxCollider2D test, Vector2 point)
    {
        bool inside;
        inside = test.bounds.Contains(point);
        return !inside;
    }
    
    static public bool IsOutside(BoxCollider2D test, Vector2 point)
    {
        Vector2    center;
        /*Vector2    direction;
        Ray2D        ray;
        RaycastHit2D hitInfo;*/
        bool       hit;
 
        // Use collider bounds to get the center of the collider. May be inaccurate
        // for some colliders (i.e. MeshCollider with a 'plane' mesh)
        center = test.bounds.center;
 
        // Cast a ray from point to center
        // direction = center - point;
        // ray = new Ray2D(point, direction);
        
        // tia chạy từ tâm cắt hình hộp 
        hit = Physics2D.Raycast(center, point);
        //hit = test.Raycast(ray, out hitInfo, direction.magnitude);
 
        // If we hit the collider, point is outside. So we return !hit
        return hit;
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
        SceneManager.LoadScene("Question02");
    }
    
}
