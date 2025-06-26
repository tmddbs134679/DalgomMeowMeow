using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 건물 설치 판별, 취소, 적용 
/// </summary>
public class BuildingPlacer : MonoBehaviour
{

    public TestBaseBuilding[] buildingSO;
    public LayerMask groundLayer;

    public GameObject BuildUi;
    public GameObject BuildTypeUI;
    public GameObject MoneyUI;
    public GameObject BuildActiontUI;
    public MoneyPreview moneyPreview;
    public GridMap gridMap;
    public BuildMap buildMap;
    public ArrayBuildPos arrayBuildPos;
    [SerializeField] private float _heightOffset = 0.5f;
    private GameObject _tempOBJ; //프리뷰 오브젝트
    private TestBaseBuilding _saveBuildingSO;

    private BuildData _buildData;
    private bool _isGold;
    private bool _isBuild;



    public void OnBuild()
    {
        BuildTypeUI.SetActive(true);
        BuildUi.SetActive(false);
    }
    //건물종류선택
    public void SelectBuildingType(int type)
    {
        buildMap.ColliderAllOff();
        Camera cam = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, cam.nearClipPlane);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out var groundHit, 1000f, groundLayer))
        {
            _saveBuildingSO = buildingSO[type];
            _tempOBJ = Instantiate(buildingSO[type].previewOBJ, new Vector3(groundHit.point.x, groundHit.point.y + _heightOffset, groundHit.point.z), Quaternion.identity);
            _tempOBJ.GetComponent<DraggableObject>().BuildActiontUI = BuildActiontUI;
            _tempOBJ.GetComponent<DraggableObject>().isDrag = true;
                        _tempOBJ.GetComponent<DraggableObject>().isLongPress= false;
            BuildActiontUI.transform.position = screenCenter;
            BuildActiontUI.SetActive(true);
            BuildTypeUI.SetActive(false);
            MoneyUI.SetActive(true);
        }
    }
    public void SetTempOBJ(GameObject tempOBJ)
    {
        _saveBuildingSO = tempOBJ.GetComponent<OwnedBuildSO>().testBaseBuilding;
        _tempOBJ = tempOBJ;
         _tempOBJ.GetComponent<DraggableObject>().BuildActiontUI = BuildActiontUI;
    }


    //건물설치재료판별
    public void CheckBuildMaterials()
    {
        if (moneyPreview.money > 0) _isGold = true;
    }

    //설치 가능한지 판별
    public void CanPlaceBuilding()
    {
        _isBuild = _tempOBJ.GetComponent<DraggableObject>().isBuild;
    }

    //설치할 장소에 설치
    public void AcceptBuild()
    {
        CheckBuildMaterials();
        CanPlaceBuilding();
        if (_isGold && _isBuild)
        {
            // -=buildingSO[type].BuildOBJ.gold;
            moneyPreview.money -= 500;
            //건물 배치후 저장
            _buildData = new BuildData();
            _buildData.posX = _tempOBJ.transform.position.x;
            _buildData.posZ = _tempOBJ.transform.position.z;
            _buildData.testBaseBuilding = _saveBuildingSO;
            arrayBuildPos.GetBuildData(_buildData);
            //배치한 자리에 있는 타일들에 isbuild체크
            _tempOBJ.GetComponent<DraggableObject>().SetTileIsBuild();
            gridMap.LoadMap();
            buildMap.LoadBuild();
        }
    }

    //설치 취소
    public void CancelBuild()
    {
        _tempOBJ.GetComponent<DraggableObject>().isDrag = false;
                _tempOBJ.GetComponent<DraggableObject>().isDrag =true;
        Destroy(_tempOBJ);
        BuildActiontUI.SetActive(false);
        BuildTypeUI.SetActive(false);
        MoneyUI.SetActive(false);
        BuildUi.SetActive(true);
        buildMap.ColliderAllOn();
        gridMap.LoadMap();
        buildMap.LoadBuild();
    }

void Update()
{
    if (_tempOBJ != null && BuildActiontUI != null && BuildActiontUI.activeSelf)
    {
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _tempOBJ.transform.position);
        BuildActiontUI.transform.position = screenPos;
    }
}
}
