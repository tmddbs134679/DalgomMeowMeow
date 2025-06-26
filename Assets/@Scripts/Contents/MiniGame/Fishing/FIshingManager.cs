using System.Collections;
using UnityEngine;

public class FIshingManager : MonoBehaviour
{
    [SerializeField] private FillSuccessGuage fillGauge;
    [SerializeField] private BaitController baitController;
    [SerializeField] private FishingRangeController fishingRangeController;

    public static FIshingManager Instance;
    private float fishingDelay = 3f;
    private float gacha = 0f;
    private float inputKeyDelay = 1f; // 낚시 입력 딜레이
    public bool isFishing = false;
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
        fillGauge.Failed += baitController.StopBait;
        fillGauge.Failed += fishingRangeController.StopFishing;
        fillGauge.Successed += baitController.StopBait;
        fillGauge.Successed += fishingRangeController.StopFishing;
    }

    private void Update()
    {
        if (!isFishing)
        {
            gacha += Time.deltaTime;
            if (gacha > fishingDelay)
            {
                //팝업 띄우고 
                gacha = 0f;
                gacha += Time.deltaTime;
                if (Input.GetMouseButton(0))
                {
                    //낚시 시작
                    isFishing = true;
                }

                if (gacha > inputKeyDelay)
                {
                    // 다시 입질
                    gacha = Random.Range(0f, 3f);
                    return;
                }

            }
        }
        }



        void OnDestroy()
    {
        fillGauge.Failed -= baitController.StopBait;
        fillGauge.Failed -= fishingRangeController.StopFishing;
        fillGauge.Successed -= baitController.StopBait;
        fillGauge.Successed -= fishingRangeController.StopFishing;
    }
}
