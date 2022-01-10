using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class LineDrawer : MonoBehaviour
{
    public List<Vector2> points;
    public float width;

    Vector2 getNormal(Vector2 start, Vector2 end) {
        Vector2 line = (end - start);
        Vector2 normal = new Vector2(line.y, -line.x);
        return normal.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        Mesh mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        if (points.Count > 1) {
            Vector2 normal = getNormal(points[0], points[1]);
            verts.Add(points[0] + normal * width / 2);
            verts.Add(points[0] - normal * width / 2);

            for (int i = 1; i < points.Count - 1; i++) {
                Vector2 backNormal = getNormal(points[i - 1], points[i]);
                Vector2 frontNormal = getNormal(points[i], points[i + 1]);
                Vector2 avgNormal = (backNormal + frontNormal) / 2;
                avgNormal = avgNormal.normalized;

                for (int x = 1; x >= -1; x -= 2) {
                    Vector2 dir = x * avgNormal;
                    float theta = Vector2.Angle(points[i - 1] - points[i], dir);
                    float mag = width / 2.0f;

                    if (theta < 45) {
                        //print("Theta:" + theta.ToString());
                        mag = (width / 2) / Mathf.Tan(Mathf.Deg2Rad * theta);
                        //print(mag);
                    }
                    if (mag > 4*width) mag = 4*width;

                    verts.Add(points[i] + dir * mag);
                }
                //verts.Add(points[i] + avgNormal * width / 2);
                //verts.Add(points[i] - avgNormal * width / 2);
            }
            
            normal = getNormal(points[points.Count-2], points[points.Count-1]);
            verts.Add(points[points.Count-1] + normal * width / 2);
            verts.Add(points[points.Count-1] - normal * width / 2);

            for (int i = 0; i < verts.Count - 2; i += 2) {
                tris.Add(i);
                tris.Add(i + 1);
                tris.Add(i + 3);

                tris.Add(i);
                tris.Add(i + 3);
                tris.Add(i + 2);
            }

            mesh.vertices = verts.ToArray();
            mesh.triangles = tris.ToArray();

            mesh.RecalculateNormals();

            GetComponent<MeshFilter>().mesh = mesh;

        }
    }
}
