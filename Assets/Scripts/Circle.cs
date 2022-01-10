using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Circle : MonoBehaviour
{
    public int resolution;
    public float radius;

    // Start is called before the first frame update
    void Awake()
    {
    
    }

    public void Draw() {
        LineRenderer outline = GetComponent<LineRenderer>();
        outline.positionCount = resolution + 2;

        for (int i = 0; i < resolution + 1; i++) {
            float t = i / (float)resolution;

            Complex timeRot = Complex.Multiply(t * 2 * 3.14159f, Complex.ImaginaryOne);
            Complex position = Complex.Multiply(radius, Complex.Exp(timeRot));

            outline.SetPosition(i, new UnityEngine.Vector2((float)position.Real, (float)position.Imaginary));
        }
        outline.SetPosition(resolution + 1, UnityEngine.Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
