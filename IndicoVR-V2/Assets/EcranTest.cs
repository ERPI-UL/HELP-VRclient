using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class EcranTest : MonoBehaviour
{
    // Start is called before the first frame update
    private Material material;
    private Texture2D texture;

    public GameObject doigt;
    // list of bounds that represent buttons
    private List<Button> buttons = new List<Button>();
    [SerializeField]
    private Vector3 min, max;
    void Start()
    {
        material = GetComponent<Renderer>().material;
        findMinMax();
        // save his texture
        // Color[] save = ((Texture2D)material.mainTexture).GetPixels();

        // create a texture that fills the plane and have the same aspect ratio as the plane
        texture = new Texture2D(256, 256);
        // texture.SetPixel(0, 0, Color.red);
        // clamp
        // texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        material.mainTexture = texture;
        // create few buttons in UV coordinates
        
        //center button
        buttons.Add(new Button(new Vector2(0.25f, 0.25f), new Vector2(0.75f, 0.75f)));
        // instantiate a cube at each corner of the button
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(min.x + (max.x - min.x) * 0.25f, 0, min.z + (max.z - min.z) * 0.25f);
        cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.transform.position = new Vector3(min.x + (max.x - min.x) * 0.75f, 0, min.z + (max.z - min.z) * 0.75f);
        cube2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        
        // draw old texture in the new one
        // texture.SetPixels(save);
        // draw the buttons
        foreach (Button button in buttons)
        {
            for (int i = (int)(button.A.x * texture.width); i < (int)(button.B.x * texture.width); i++)
            {
                for (int j = (int)(button.A.y * texture.height); j < (int)(button.B.y * texture.height); j++)
                {
                    texture.SetPixel(i, j, Color.green);
                }
            }
        }
        // draw x axis of the texture in red
        for (int i = 0; i < texture.width; i++)
        {
            texture.SetPixel(i, (int)(texture.height / 4), Color.red);
        }
        // draw y axis of the texture in green
        for (int j = 0; j < texture.height; j++)
        {
            texture.SetPixel((int)(texture.width / 4), j, Color.green);
        }

    }

    [BurstCompile]
    private void findMinMax()
    {
        // get all point of the mesh
        Vector3[] vertices = GetComponent<MeshFilter>().mesh.vertices;
        // find the min and max corners of the mesh
        Vector3 min = vertices[0];
        Vector3 max = vertices[0];
        foreach (Vector3 v in vertices)
        {
            if (v.x < min.x)
            {
                min.x = v.x;
            }
            if (v.y < min.y)
            {
                min.y = v.y;
            }
            if (v.z < min.z)
            {
                min.z = v.z;
            }
            if (v.x > max.x)
            {
                max.x = v.x;
            }
            if (v.y > max.y)
            {
                max.y = v.y;
            }
            if (v.z > max.z)
            {
                max.z = v.z;
            }
        }
        // draw the min and max corners of the mesh

        // to world coordinates
        min = transform.TransformPoint(min);
        max = transform.TransformPoint(max);
        this.min = min;
        this.max = max;
        // spawn a cube at the min corner
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = min;
        // spawn a cube at the max corner
        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.transform.position = max;
        // // to UV coordinates
        this.min = new Vector3(min.x / transform.localScale.x, min.y / transform.localScale.y, min.z / transform.localScale.z);
        this.max = new Vector3(max.x / transform.localScale.x, max.y / transform.localScale.y, max.z / transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        // launch a ray from the finger to the plane
        Ray ray = new Ray(doigt.transform.position, doigt.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector2 uv = hit.textureCoord;
            // draw a gray circle at the collision point
            // for (int i = (int)(uv.x * texture.width) - 10; i < (int)(uv.x * texture.width) + 10; i++)
            // {
            //     for (int j = (int)(uv.y * texture.height) - 10; j < (int)(uv.y * texture.height) + 10; j++)
            //     {
            //         texture.SetPixel(i, j, Color.gray);
            //     }
            // }
            // for (int x = 0; x < texture.width; x++)
            // {
            //     for (int y = 0; y < texture.height; y++)
            //     {
            //         float dx = x - uv.x * texture.width;
            //         float dy = y - uv.y * texture.height;

            //         if (Mathf.Sqrt(dx * dx + dy * dy) <= 10)
            //         {
            //             texture.SetPixel(x, y, Color.black);
            //         }
            //     }
            // }

            // get hit in locale space of the plane
            Vector3 hitLocal;
            hitLocal = transform.InverseTransformPoint(hit.point);
            // remap to UV coordinates
            Vector2 uv2;
            uv2.x = Unity.Mathematics.math.remap(0, max.z, 0, 1, hitLocal.z);
            uv2.y = Unity.Mathematics.math.remap(0, max.x, 0, 1, hitLocal.x);
            Debug.Log(uv2);
            // Debug.Log("HitLocal Z : "+hitLocal.z + " UV X : "+uv2.x);
            // Debug.Log("HitLocal X : "+hitLocal.x + " UV Y : "+uv2.y);
            foreach (Button button in buttons)
            {
                if (uv2.x > button.A.x && uv2.x < button.B.x && uv.y > button.A.y && uv2.y < button.B.y)
                {
                    Debug.Log("Button pressed");
                    // draw the button in blue
                    for (int i = (int)(button.A.x * texture.width); i < (int)(button.B.x * texture.width); i++)
                    {
                        for (int j = (int)(button.A.y * texture.height); j < (int)(button.B.y * texture.height); j++)
                        {
                            texture.SetPixel(i, j, Color.blue);
                        }
                    }
                }else{
                    // white
                    for (int i = (int)(button.A.x * texture.width); i < (int)(button.B.x * texture.width); i++)
                    {
                        for (int j = (int)(button.A.y * texture.height); j < (int)(button.B.y * texture.height); j++)
                        {
                            texture.SetPixel(i, j, Color.white);
                        }
                    }
                }
            }

            // apply the texture
            texture.Apply();
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 0.01f);

        }

    }
    bool isInside(Vector2 uv, Vector2 A, Vector2 B)
    {
        if (uv.x > A.x && uv.x < B.x && uv.y > A.y && uv.y < B.y)
        {
            return true;
        }
        return false;
    }
    [BurstCompile]
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        Ray ray = new Ray(doigt.transform.position, doigt.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector2 uv = hit.textureCoord;
            // draw a gray circle at the collision point
            // for (int i = (int)(uv.x * texture.width) - 10; i < (int)(uv.x * texture.width) + 10; i++)
            // {
            //     for (int j = (int)(uv.y * texture.height) - 10; j < (int)(uv.y * texture.height) + 10; j++)
            //     {
            //         texture.SetPixel(i, j, Color.gray);
            //     }
            // }
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    float dx = x - uv.x * texture.width;
                    float dy = y - uv.y * texture.height;

                    if (Mathf.Sqrt(dx * dx + dy * dy) <= 10)
                    {
                        texture.SetPixel(x, y, Color.black);
                    }
                }
            }
        }
        texture.Apply();
    }
    void OnCollisionExit(Collision collision)
    {
        Debug.Log("collision");

        Ray ray = new Ray(doigt.transform.position, doigt.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // find the collision point in UV coordinates
            Vector2 uv = hit.textureCoord;
            // draw a red pixel at the collision point
            texture.SetPixel((int)(uv.x * texture.width), (int)(uv.y * texture.height), Color.red);
            Debug.Log("uv.x = " + uv.x + " uv.y = " + uv.y);
            foreach (Button button in buttons)
            {
                // draw the button in blue
                for (int i = (int)(button.A.x * texture.width); i < (int)(button.B.x * texture.width); i++)
                {
                    for (int j = (int)(button.A.y * texture.height); j < (int)(button.B.y * texture.height); j++)
                    {
                        texture.SetPixel(i, j, Color.green);
                    }
                }

            }
            // search the buttons that contain the hit point
            foreach (Button button in buttons)
            {
                if (uv.x > button.A.x && uv.x < button.B.x && uv.y > button.A.y && uv.y < button.B.y)
                {
                    // draw the button in blue
                    for (int i = (int)(button.A.x * texture.width); i < (int)(button.B.x * texture.width); i++)
                    {
                        for (int j = (int)(button.A.y * texture.height); j < (int)(button.B.y * texture.height); j++)
                        {
                            texture.SetPixel(i, j, Color.blue);
                        }
                    }
                }
            }
        }
        // draw the ray in red
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 0.1f);
        texture.Apply();
    }
    private struct Button
    {
        public Vector2 A;
        public Vector2 B;
        public Button(Vector2 A, Vector2 B)
        {
            this.A = A;
            this.B = B;
        }
    }
}
