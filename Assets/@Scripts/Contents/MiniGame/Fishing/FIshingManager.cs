using System.Collections;
using UnityEngine;

public class FIshingManager : MonoBehaviour
{
    [SerializeField] private FillSuccessGuage fillGauge;
    [SerializeField] private BaitController baitController;
    [SerializeField] private FishingRangeController fishingRangeController;

    public static FIshingManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
        fillGauge.Success += baitController.StopBait;
        fillGauge.Success += fishingRangeController.StopFishing;

    }

    void OnDestroy()
    {
        fillGauge.Success -= baitController.StopBait;
        fillGauge.Success -= fishingRangeController.StopFishing;
    }
}
