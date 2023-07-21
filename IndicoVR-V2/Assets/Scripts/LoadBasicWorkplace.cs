using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using AimXRToolkit;
using AimXRToolkit.Managers;
using AimXRToolkit.Models;

public class LoadBasicWorkplace : MonoBehaviour
{
    // Start is called before the first frame update
    public AimXRToolkit.Managers.WorkPlaceManager workplaceManager;
    public int workplaceId = 2;
    async void Start()
    {
        if (workplaceManager == null)
        {
            Debug.LogError("no workplace manager found");
        }

        if (AimXRManager.Instance.GetWorkplaceId() > 0)
        {
            workplaceManager.SetWorkplace(await DataManager.GetInstance().GetWorkplaceAsync(AimXRManager.Instance.GetWorkplaceId()));

        }
        else
        {
            workplaceManager.SetWorkplace(await DataManager.GetInstance().GetWorkplaceAsync(workplaceId));
        }
        await workplaceManager.Spawn();
        Debug.Log(workplaceManager.GetArtifacts().Count);
        // wait for user token to not be null or empty async
        while (AimXRManager.Instance.GetUser() == null || string.IsNullOrEmpty(AimXRManager.Instance.GetUser().token))
        {
            await Task.Delay(1000);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(workplaceManager.GetArtifacts().Count);
    }
}
