using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class DrawLine : MonoBehaviour
{
    [SerializeField] private LineRenderer linePrefab;
    private LineRenderer lineRenderer;
    
    private EdgeCollider2D edgeColl;
    private List<Vector2> points;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            lineRenderer = Instantiate(linePrefab);

        if (Input.GetMouseButton(0))
        {
            Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(tempFingerPos, lineRenderer.GetPosition(lineRenderer.positionCount - 1)) > .1f)
            {
                AddPoint(tempFingerPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 tempFingerPoses = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            List<Vector2> points = new List<Vector2>();
            if (Vector2.Distance(tempFingerPoses, lineRenderer.GetPosition(lineRenderer.positionCount - 1)) > .1f)
            {
                AddPoint(tempFingerPoses);
                points.Add(tempFingerPoses);
            }

            bool correct = true;
            /*phan thang thua*/
//            foreach (Vector2 item in points)
//            {
//                if(!item bentrong)
//                {
//                    correct = false;
//                    break;
//                }
//            }
//
//            if (correct = 0)
//            {
//                // thang
//            }
//            else if (correct = 1)
//            {
//                // thua
//            }

        }
    }
    
    void AddPoint(Vector2 newFingerPos)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
    }

}
