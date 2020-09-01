using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Transform pointPrefab;
    [Range(10, 100)]
    public int resolution = 10;
    Transform[] points;
    //[Range(0, 1)]
    //public int function;
    public GraphFunctionName function;

    static GraphFunction[] functions = {
            SineFunction, Sine2DFunction, MultiSineFunction, MultiSine2DFunction,
            Ripple, Cylinder, Sphere, Torus
        };

    const float pi = Mathf.PI;

    private void Awake()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        //Vector3 position = Vector3.zero;
        points = new Transform[resolution * resolution];

        //for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        //{
            //if (x == resolution)
            //{
            //    x = 0;
            //    z++;
            //}
        //for (int i = 0, z = 0; z < resolution; z++)
        //{
        //    position.z = (z + 0.5f) * step - 1f;
        //    for (int x = 0; x < resolution; i++, x++)
        //    {
        //        Transform point = Instantiate(pointPrefab);
        //        position.x = (x + 0.5f) * step - 1f;
        //        point.localPosition = position;
        //        point.localScale = scale;
        //        point.SetParent(transform, false);
        //        points[i] = point;
        //    }
        //}
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
    }

    private void Update()
    {
        float t = Time.time;
        GraphFunction f = functions[(int)function];
        //if (function == 0)
        //{
        //    f = SineFunction;
        //}
        //else
        //{
        //    f = MultiSineFunction;
        //}


        //for (int i = 0; i < points.Length; i++)
        //{
        //    Transform point = points[i];
        //    Vector3 position = point.localPosition;
        //    //position.y = Mathf.Sin(Mathf.PI * (position.x + Time.time));
        //    //if (function == 0)
        //    //{
        //    //    position.y = SineFunction(position.x, t);
        //    //}
        //    //else
        //    //{
        //    //    position.y = MultiSineFunction(position.x, t);
        //    //}
        //    position.y = f(position.x, position.z, t);
        //    point.localPosition = position;
        //}
        float step = 2f / resolution;
        for (int i = 0, z = 0; z < resolution; z++)
        {
            float v = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++)
            {
                float u = (x + 0.5f) * step - 1f;
                points[i].localPosition = f(u, v, t);
            }
        }
    }

    //static float SineFunction(float x, float z, float t)
    //{
    //    return Mathf.Sin(pi * (x + t));
    //}

    //static float Sine2DFunction(float x, float z, float t)
    //{
    //    //return Mathf.Sin(pi * (x + z + t));
    //    float y = Mathf.Sin(pi * (x + t));
    //    y += Mathf.Sin(pi * (z + t));
    //    y *= 0.5f;
    //    return y;
    //}

    //static float MultiSineFunction(float x, float z, float t)
    //{
    //    float y = Mathf.Sin(pi * (x + t));
    //    y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
    //    y *= 2f / 3f;
    //    return y;
    //}

    //static float MultiSine2DFunction(float x, float z, float t)
    //{
    //    float y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
    //    y += Mathf.Sin(pi * (x + t));
    //    y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
    //    y *= 1f / 5.5f;
    //    return y;
    //}

    //static float Ripple(float x, float z, float t)
    //{
    //    float d = Mathf.Sqrt(x * x + z * z);
    //    float y = Mathf.Sin(pi * (4f * d - t));
    //    y /= 1f + 10f * d;
    //    return y;
    //}

    static Vector3 SineFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.z = z;
        return p;
    }

    static Vector3 Sine2DFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(pi * (z + t));
        p.y *= 0.5f;
        p.z = z;
        return p;
    }

    static Vector3 MultiSineFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
        p.y *= 2f / 3f;
        p.z = z;
        return p;
    }

    static Vector3 MultiSine2DFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
        p.y += Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
        p.y *= 1f / 5.5f;
        p.z = z;
        return p;
    }

    static Vector3 Ripple(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        float d = Mathf.Sqrt(x * x + z * z);
        p.y = Mathf.Sin(pi * (4f * d - t));
        p.y /= 1f + 10f * d;
        p.z = z;
        return p;
    }

    static Vector3 Cylinder(float u, float v, float t)
    {
        Vector3 p;
        //float r = 1f + Mathf.Sin(6f * pi * u) * 0.2f;
        //float r = 1f + Mathf.Sin(2f * pi * v) * 0.2f;
        float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v + t)) * 0.2f;
        p.x = r * Mathf.Sin(pi * u);
        p.y = v;
        p.z = r * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p;
        //float r = Mathf.Cos(pi * 0.5f * v);
        //p.x = r * Mathf.Sin(pi * u);
        //p.y = Mathf.Sin(pi * 0.5f * v);
        //p.z = r * Mathf.Cos(pi * u);
        float r = 0.8f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
        r += Mathf.Sin(pi * (4f * v + t)) * 0.1f;
        float s = r * Mathf.Cos(pi * 0.5f * v);
        p.x = s * Mathf.Sin(pi * u);
        p.y = r * Mathf.Sin(pi * 0.5f * v);
        p.z = s * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 Torus(float u, float v, float t)
    {
        Vector3 p;
        //float r1 = 1f;
        //float r2 = 0.5f;
        float r1 = 0.65f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
        float r2 = 0.2f + Mathf.Sin(pi * (4f * v + t)) * 0.05f;
        float s = r2 * Mathf.Cos(pi * v) + r1;
        p.x = s * Mathf.Sin(pi * u);
        p.y = r2 * Mathf.Sin(pi * v);
        p.z = s * Mathf.Cos(pi * u);
        return p;
    }
}

