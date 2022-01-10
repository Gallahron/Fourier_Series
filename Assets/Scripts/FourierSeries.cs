using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class FourierSeries : MonoBehaviour
{
    LineRenderer outline;
    LineDrawer lines;
    public Circle circle;
    public Circle final;

    public UnityEngine.Vector2[] input;
    public List<Complex> solution = new List<Complex>(); //{ new Complex(1,1), new Complex(0,2)};

    List<Circle> circles = new List<Circle>();

    public int outlineVertices = 1000;
    public int noCircles = 20;
    //public int negativeCircles = 5;

    public int inputThresh = 60;

    [Range(0,1)]
    public float negativePerc;

    public float secsForRot;

    public Slider circleSlider;

    public void Recalculate() {
        if (input.Length > inputThresh) {
            OnDisable();
            noCircles = (int)circleSlider.value + 1;
            if (gameObject.activeSelf) {
                OnEnable();
            }
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        if (input.Length > inputThresh) {
            outline = GetComponent<LineRenderer>();
            outline.positionCount = outlineVertices;
            solution = new List<Complex>();
            circles = new List<Circle>();

            print("DING!!!!!!!");
            //lines = transform.GetChild(0).GetComponent<LineDrawer>();
            Solve();

            for (int i = 0; i < noCircles - 1; i++) {
                Circle instance = Instantiate(circle.gameObject, transform).GetComponent<Circle>();
                instance.radius = complexToVector2(solution[i]).magnitude;
                instance.Draw();
                circles.Add(instance);
            }

            Circle finalCircle = Instantiate(final.gameObject, transform).GetComponent<Circle>();
            circles.Add(finalCircle);
        }
    }

    private void OnDisable() {
        for (int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }
        circles = new List<Circle>();
    }

    UnityEngine.Vector2 complexToVector2(Complex coord) {
        UnityEngine.Vector2 result = new UnityEngine.Vector2((float)coord.Real, (float)coord.Imaginary);
        return result;
    }
    Complex vector2ToComplex(UnityEngine.Vector2 coord) {
        Complex result = new Complex(coord.x, coord.y);
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        Complex timeRot = Complex.Multiply(2 * 3.14159f, Complex.ImaginaryOne);
        int negativeCircles = (int)(noCircles * negativePerc);

        for (int i = 0; i < outlineVertices; i++) {
            Complex val = new Complex(0, 0);
            float t = i / (float)outlineVertices;

            for (int j = -negativeCircles; j < noCircles - negativeCircles; j++) {
                Complex term = Complex.Multiply(solution[j + negativeCircles], Complex.Exp(Complex.Multiply(j * t, timeRot)));
                val = Complex.Add(val, term);
            }

            outline.SetPosition(i, complexToVector2(val));
        }
        
        DrawLines();
        circles[circles.Count - 1].gameObject.SetActive(true);
    }

    void DrawLines() {
        Complex timeRot = Complex.Multiply(2 * 3.14159f, Complex.ImaginaryOne);
        Complex val = new Complex(0, 0);

        int negativeCircles = (int)(noCircles * negativePerc);

        float t = Time.time / secsForRot;
        t = t - (int)t;

        print(solution.Count);
        //lines.points = new List<UnityEngine.Vector2>();

        UnityEngine.Vector2 oldPos = UnityEngine.Vector2.zero;
        for (int circle = -negativeCircles; circle < noCircles - negativeCircles; circle++) {
            int index = circle + negativeCircles;

            Complex term = Complex.Multiply(solution[index], Complex.Exp(Complex.Multiply(circle * t, timeRot)));
            val = Complex.Add(val, term);

            //lines.points.Add(oldPos);

            circles[index].transform.position = oldPos;
            circles[index].transform.right = complexToVector2(val) - oldPos;
            oldPos = complexToVector2(val);
        }
    }

    void Solve() {
        // outline.positionCount = input.Length;
        int negativeCircles = (int)(noCircles * negativePerc);

        for (int n = -negativeCircles; n < noCircles - negativeCircles; n++) {
            Complex term = new Complex(0, 0);
            float timeStep = 1/(float)input.Length;
            for (int t = 0; t < input.Length; t++) {
                UnityEngine.Vector2 pos = input[t] * timeStep;
                //print(timeStep.ToString() + " " + (t * timeStep).ToString());
                Complex rot = Complex.Multiply(-n * t * timeStep * 2 * 3.14159f, Complex.ImaginaryOne);
                Complex mod = Complex.Exp(rot);
                Complex point = Complex.Multiply(vector2ToComplex(pos), mod);
                term = Complex.Add(term, point);
            }
            solution.Add(term);
            //print(term);
        }
    }
}
