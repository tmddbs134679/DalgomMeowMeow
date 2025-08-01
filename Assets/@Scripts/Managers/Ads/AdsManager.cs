using System;
using UnityEngine;
using UnityEngine.Advertisements;
/*
public class AdsManager : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    //[SerializeField] private string androidGameId = "ca-app-pub-3940256099942544/5354046379"; // 실제 ID로 교체
    [SerializeField] private string interstitialAdUnitId = "Interstitial_Android";
    [SerializeField] private bool testMode = true;
    [SerializeField] private string rewardedAdUnitId = "Rewarded_Android";

    private bool _isAdLoaded = false;


    private string _gameId;

    public void Init()  //반드시 먼저 실행시켜야됨
    {

#if UNITY_EDITOR
        _gameId = "5901065"; // Unity Ads 대시보드에서 에디터용 ID
#elif UNITY_ANDROID
    _gameId = "5901065";
#elif UNITY_IOS
    _gameId = "5901064";
#endif
        Advertisement.Initialize(_gameId, testMode, this);

    }
    // 초기화 콜백
    public void OnInitializationComplete()
    {
        Advertisement.Load(interstitialAdUnitId, this); // 광고 미리 로드
    }


    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == interstitialAdUnitId)
        {
            _isAdLoaded = true;
        }
    }

    public void ShowRewardedAd(Action onRewarded)
    {
        if (_isAdLoaded)
        {
            Advertisement.Show(rewardedAdUnitId, this);
            _onRewardedCallback = onRewarded;
            _isAdLoaded = false; // 다음 광고를 위해 초기화
        }
        else
        {
            Advertisement.Load(interstitialAdUnitId, this);
        }
    }

    private Action _onRewardedCallback;

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == rewardedAdUnitId)
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                _onRewardedCallback?.Invoke(); // 보상 콜백 실행
                Advertisement.Load(rewardedAdUnitId, this); // 다음 광고 로드
                _isAdLoaded = true;
            }
            else
            {
            }

            _onRewardedCallback = null; // 콜백 해제
            Advertisement.Load(rewardedAdUnitId, this); // 다음 광고 로드
        }
    }



    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Managers.Debug.LogError($"광고 초기화 실패: {error.ToString()} - {message}", Define.EDebugType.AD);
    }
    

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Managers.Debug.LogError($"광고 로드 실패: {placementId} - {error.ToString()} - {message}", Define.EDebugType.AD);
    }

    // 광고 실행 콜백
    

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Managers.Debug.LogError($"광고 재생 실패: {placementId} - {error.ToString()} - {message}", Define.EDebugType.AD);
    }

    public void OnUnityAdsShowStart(string placementId) { } //광고 시작 시 호출
    public void OnUnityAdsShowClick(string placementId) { } //광고 클릭 시 호출
}
*/