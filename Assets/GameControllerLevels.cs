using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameControllerLevels : MonoBehaviour
{
    public GameObject questionImage; // Using path
    public GameObject answerImage; // Using path
    public GameObject checkBoxes; // Using path
    
    [SerializeField] private LineRenderer linePrefab; // drag and drop
    [SerializeField] private BoxCollider2D winBox; // received

    [SerializeField] private CircleCollider2D[] checkBox;

    private LineRenderer lineRenderer; // need to be deleted afterward
    public GameObject AnswerImageInstant; // need to be deleted afterward

    private bool gameHasEnded = false;
    [SerializeField] private float restartDelay = 2f;

    private GameObject questImg;
    private GameObject questImgCheck;
    
    private List<Vector2> points = new List<Vector2>();

    private bool inside = true;
    private bool allChecked = false;

    private int countCheck;
    
    /*Levels region*/
    [SerializeField] private int level;
    private int levelSetting;

    private void Awake()
    {
        if (level == 0)
        {
            level = 1;
        }
    }

    private void Reset()
    {
        questionImage = LoadPrefabFromFile(ConcatSomething("Question", level)) as GameObject; // Using path
        answerImage = LoadPrefabFromFile(ConcatSomething("Answer", level)) as GameObject; // Using path
        checkBoxes = LoadPrefabFromFile(ConcatSomething("Boxe", level)) as GameObject; // Also using folder named "Boxes" path (similar to "Z.Answer" in examples) 

        questImg = Instantiate(questionImage, Vector3.zero, Quaternion.identity); // Enable prefab
        questImgCheck = Instantiate(checkBoxes);
        
        winBox = questImg.GetComponentInChildren<BoxCollider2D>(); // Using prefab that imported from path
        checkBox = questImgCheck.GetComponentsInChildren<CircleCollider2D>(); // Using prefab that imported from path
    }

    void Start()
    {
        questionImage = LoadPrefabFromFile(ConcatSomething("Question", level)) as GameObject; // Using path
        answerImage = LoadPrefabFromFile(ConcatSomething("Answer", level)) as GameObject; // Using path
        checkBoxes = LoadPrefabFromFile(ConcatSomething("Boxe", level)) as GameObject; // Also using folder named "Boxes" path (similar to "Z.Answer" in examples) 

        questImg = Instantiate(questionImage, Vector3.zero, Quaternion.identity); // Enable prefab
        questImgCheck = Instantiate(checkBoxes);
        
        winBox = questImg.GetComponentInChildren<BoxCollider2D>(); // Using prefab that imported from path
        checkBox = questImgCheck.GetComponentsInChildren<CircleCollider2D>(); // Using prefab that imported from path
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
            for (int j=0; j < points.Count; j++) // Check first and last boxes
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
            foreach (Vector2 item in points) // Check each couple of boxes
            {
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
            if (inside && allChecked && (countCheck >= checkBox.Length)) // Ticket box (passed of NOT)
            {
                Debug.Log("correct");
                //load answer
                DestroyImmediate(questImg.gameObject);
                DestroyImmediate(questImgCheck.gameObject);

                 AnswerImageInstant = Instantiate(answerImage.gameObject, Vector3.zero, Quaternion.identity);
                
                /* level up */
                level = PlayerPrefs.GetInt(null, level+1);

                Invoke("NextQuestion", restartDelay);
            }
            else // Yet to win
            {
                Debug.Log("incorrect");
                
                Invoke("ReLoad", restartDelay);
            }

            lineRenderer.positionCount = 0;
            DestroyImmediate(lineRenderer.gameObject);
        }
    }
    
    private void FixedUpdate()
    {
        if (level > levelSetting)
        {
            Invoke("LoadLastScence", restartDelay);
        }
    }

    // Win
    public void NextQuestion()
    {
        DestroyImmediate(AnswerImageInstant, true);
        Reset();
    }

    // Yet to win
    public void ReLoad()
    {
        DestroyImmediate(questImg.gameObject);
        DestroyImmediate(questImgCheck.gameObject);
        Reset();
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

    private UnityEngine.Object LoadPrefabFromFile(string filename)
    {
        Debug.Log("Trying to load LevelPrefab from file ("+filename+ ")...");
        var loadedObject = Resources.Load(filename);
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        return loadedObject;
    }

    static string ConcatSomething(string QorA, int level)
    {
        /*"Question/" + level.ToString() + ".png"*/
        string concat = $"Levels/{QorA}s/{level.ToString()}";
        Debug.Log(concat);
        return concat;
    }

    private void LoadLastScence()
    {
        SceneManager.LoadScene("Question02");
    }
}