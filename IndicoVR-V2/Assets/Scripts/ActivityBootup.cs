using AimXRToolkit.Managers;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

public class ActivityBootup : MonoBehaviour
{
    [SerializeField] private int defaultWorkplaceId;
    [SerializeField] private int defaultActivityId;

    [SerializeField] private ActivityManager activityManager;
    [SerializeField] private WorkPlaceManager workplaceManager;

    [SerializeField] private ActivityLoading loading;
    [SerializeField] private UnityEvent onBooted;
    // Start is called before the first frame update
    async void Start()
    {
        if (activityManager == null || workplaceManager == null)
        {
            Debug.Log("Please set reference to the managers in the inspector");
        }
        int activityId = AimXRManager.Instance.GetActivityId();
        int workplaceId = AimXRManager.Instance.GetWorkplaceId();
        if (activityId > 0)
        {
            activityManager.SetActivity(await DataManager.GetInstance().GetActivityAsync(activityId));

        }
        else
        {
            activityManager.SetActivity(await DataManager.GetInstance().GetActivityAsync(defaultActivityId));
        }
        if (workplaceId > 0)
        {
            workplaceManager.SetWorkplace(await DataManager.GetInstance().GetWorkplaceAsync(workplaceId));

        }else{
            workplaceManager.SetWorkplace(await DataManager.GetInstance().GetWorkplaceAsync(defaultWorkplaceId));
        }
        try
        {
            await workplaceManager.Spawn();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
        await Task.Delay(2000);
        loading.gameObject.SetActive(false);
        onBooted.Invoke();
        _ = await activityManager.NextAction();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void NextAction()
    {
        Debug.Log("Next action");
        _ = activityManager.NextAction();
        Debug.Log("Next action done");
    }
    public void PreviousAction()
    {
        _ = activityManager.PreviousAction();
    }

    public void ReloadActivity()
    {
        // _ = activityManager.ReloadActivity();
        // reload the scene for the moment
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
    }
}
