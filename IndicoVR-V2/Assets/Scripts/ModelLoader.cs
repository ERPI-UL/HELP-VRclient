using GLTFast;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ModelLoader : AimXRToolkit.LoaderSource
{
    public async override Task<GameObject> LoadGlb(string uri)
    {
        UnityWebRequest www = UnityWebRequest.Get(uri);
        TaskCompletionSource<UnityWebRequest> tcs = new TaskCompletionSource<UnityWebRequest>();
        www.SendWebRequest().completed += operation =>
        {
            tcs.SetResult(www);
        };
        await tcs.Task;
        // get bytes
        var bytes = www.downloadHandler.data;
        ImportSettings importSettings = new ImportSettings()
        {
            NodeNameMethod = NameImportMethod.OriginalUnique
        };
        var gltf = new GLTFast.GltfImport();
        var success = await gltf.LoadGltfBinary(bytes, importSettings: importSettings);
        if (!success)
        {
            Debug.LogError("Loading glTF failed!");
        }

        var o = new GameObject("machine");
        
        await gltf.InstantiateMainSceneAsync(o.transform);
        // on remonte dans la machine tout ce qui est dans la scene

        var scene = o.transform.GetChild(0);
       
        for (int i = scene.childCount - 1; i >= 0; i--)
        {
            var child = scene.GetChild(i);
            child.SetParent(o.transform);
        }

        // on supprime la scene
        Destroy(scene.gameObject);
        return o;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
