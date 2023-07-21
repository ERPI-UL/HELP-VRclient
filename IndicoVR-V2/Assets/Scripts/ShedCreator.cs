using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ShedCreator : MonoBehaviour
{
    private const float tileSize = 4.0f;
    private const float shredSize = 10.0f;
    // Start is called before the first frame update
    public GameObject floor;
    public GameObject RoofWithPoleSupport;
    public GameObject Roof;
    public GameObject Pole;
    public GameObject RoofAndPole;

    public Vector3 position;
    public Vector3 rotation;


    private List<GameObject> _shred;

    private List<Bounds> _boundsList;
    void Start()
    {
        _shred = new List<GameObject>();
        // test that all prefab has been set 
        if (floor == null || RoofWithPoleSupport == null || Roof == null || Pole == null)
        {
            Debug.LogError("Please set all prefab in ShedCreator");
            return;
        }
        Spawn(position);
    }

    // Update is called once per frame
    void Update()
    {
        /* foreach (GameObject instantiated in _shred)
        {
            Destroy(instantiated);
        }
        _shred.Clear();
        Spawn(toAvoid, 10, 0, 10); */
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="machines">GameObjects to avoid</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void Spawn(Vector3 position)
    {
        InstantiateShred(position - new Vector3(shredSize * tileSize / 2, 0, shredSize * tileSize / 2));
    }

    public bool IsInAnyArtifact(Vector3 vec)
    {
        foreach (var b in _boundsList)
        {
            if (b.Contains(vec))
            {
                Debug.Log("Is in artifact");
                return true;
            }
        }
        return false;
    }
    [BurstCompile]
    public void InstantiateShred(Vector3 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;
        for (int i = 0; i < shredSize; i++)
        {
            for (int j = 0; j < shredSize; j++)
            {
                InstantiateChunk(pos + new Vector3(tileSize * i + tileSize, 0, tileSize * j), (i % 2) == 0 && (j % 4) == 0);
            }
        }
    }

    private void InstantiateChunk(Vector3 vec, bool pole = false)
    {
        _shred.Add(InstantiateFloor(vec));
        if (pole)
        {
            /* _shred.Add(InstantiateRoofWithPoleSupport(x, y + 4, z));
            _shred.Add(InstantiatePole(x, y, z));
            */
            _shred.Add(InstantiateRoofAndPole(vec));
        }
        else
        {
            _shred.Add(InstantiateRoof(vec + new Vector3(0, 4.0f, 0)));
        }
    }

    private GameObject InstantiateRoofAndPole(Vector3 vec)
    {
        return Instantiate(RoofAndPole, vec, Quaternion.identity);
    }
    private GameObject InstantiateFloor(Vector3 vec)
    {
        return Instantiate(floor, vec, Quaternion.identity);
    }

    private GameObject InstantiateRoofWithPoleSupport(int x, int y, int z)
    {
        return Instantiate(RoofWithPoleSupport, new Vector3(x, y, z), Quaternion.identity);
    }

    private GameObject InstantiateRoof(Vector3 vec)
    {
        return Instantiate(Roof, vec, Quaternion.identity);
    }

    private GameObject InstantiatePole(Vector3 vec)
    {
        return Instantiate(Pole, vec, Quaternion.identity);
    }

    private Bounds GetMaxBounds(GameObject g)
    {
        var renderers = g.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(g.transform.position, Vector3.zero);
        var b = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            b.Encapsulate(r.bounds);
        }

        Debug.DrawLine(b.min, b.max, Color.red, 0.16f);
        return b;
    }
}
