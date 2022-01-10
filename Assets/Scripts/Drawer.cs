using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drawer : MonoBehaviour
{
    LineRenderer outline;
    public FourierSeries renderer;
    public Vector2 avgSpeed;
    public float doubleThresh;

    List<Vector2> positions;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<LineRenderer>();

        Application.runInBackground = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            if (Input.GetMouseButtonDown(0)) {
                outline.positionCount = 0;
                outline.enabled = true;
                renderer.gameObject.SetActive(false);
                positions = new List<Vector2>();
            }

            if (Input.GetMouseButton(0)) {
                outline.positionCount++;

                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                outline.SetPosition(outline.positionCount - 1, pos);
                positions.Add(pos);
            }
            if (Input.GetMouseButtonUp(0)) {
                Vector2 avg = new Vector2();
                for (int i = 0; i < positions.Count; i++) avg += positions[i];
                avg /= positions.Count;

                Vector2 skipStart = positions[positions.Count - 1];
                Vector2 skipEnd = positions[0];

                Vector2 dir = (skipEnd - skipStart);
                float speed = calcAvgSpeed();
                for (int i = 0; i < dir.magnitude / speed; i++) {
                    positions.Add(skipStart + i * dir.normalized * speed);
                }

                while (calcAvgSpeed() > doubleThresh) {
                    int posMod = 0;
                    int maxPos = positions.Count;
                    for (int i = 1; i < maxPos; i++) {
                        Vector2 interVal = (positions[i + posMod - 1] + positions[i + posMod]) / 2;
                        positions.Insert(i + posMod, interVal);

                        posMod++;
                    }
                }

                //for (int i = 0; i < positions.Count; i++) positions[i] -= avg;


                renderer.input = positions.ToArray();
                renderer.gameObject.SetActive(true);
                outline.enabled = false;
            }
        }
    }

    float calcAvgSpeed() {
        float dist = 0;
        for (int i = 1; i < positions.Count; i++) {
            dist += (positions[i] - positions[i - 1]).magnitude;
        }
        dist /= positions.Count;
        print(dist);
        return dist;
    }
}
