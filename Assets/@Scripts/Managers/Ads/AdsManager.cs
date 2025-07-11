using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidGameId = "1234567"; // 실제 ID로 교체
    [SerializeField] private string interstitialAdUnitId = "Interstitial_Android";
    [SerializeField] private bool testMode = true;
    private bool _isAdLoaded = false;


    private string _gameId;

    public void Init()  //반드시 먼저 실행시켜야됨
    {
#if UNITY_ANDROID
        _gameId = androidGameId;
#elif UNITY_IOS
        _gameId = "ios게임아이디";
#endif
        Advertisement.Initialize(_gameId, testMode, this);  //추후에 testMode를 false로 변경하여 실제 광고로 전환 가능
    }
    // 초기화 콜백
    public void OnInitializationComplete()
    {
        Debug.Log("광고 초기화 완료");
        Advertisement.Load(interstitialAdUnitId, this); // 광고 미리 로드
    }


    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == interstitialAdUnitId)
        {
            _isAdLoaded = true;
            Debug.Log("광고 로딩 완료!");
        }
    }

    public void ShowAd()
    {
        if (_isAdLoaded)
        {
            Advertisement.Show(interstitialAdUnitId, this);
            _isAdLoaded = false; // 다음 광고를 위해 다시 로딩 필요
        }
        else
        {
            Debug.Log("광고 아직 로딩 안됨");
        }
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("광고 시청 완료");

            //광고 보상 로직



        }
        else
        {
            Debug.Log("광고 스킵됨 또는 실패");
        }
        Advertisement.Load(interstitialAdUnitId, this); // 다음 광고를 위해 다시 로드
    }




    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"광고 초기화 실패: {error.ToString()} - {message}");
    }
    

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"광고 로드 실패: {placementId} - {error.ToString()} - {message}");
    }

    // 광고 실행 콜백
    

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"광고 재생 실패: {placementId} - {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId) { } //광고 시작 시 호출
    public void OnUnityAdsShowClick(string placementId) { } //광고 클릭 시 호출
}
