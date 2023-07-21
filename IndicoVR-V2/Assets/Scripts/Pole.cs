using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pole : MonoBehaviour
{
    public GameObject roofPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "machine")
        {
            Debug.Log("machine in the way of pole deletion");
            if (roofPrefab != null)
                Instantiate(roofPrefab, this.transform.position + new Vector3(0, 4, 0), this.transform.rotation);
            Destroy(this.transform.parent.gameObject);
        }
    }
}
