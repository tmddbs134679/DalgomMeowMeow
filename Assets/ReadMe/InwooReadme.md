## 1. 맵 및 타일 저장/불러오기 

이 기능은 게임 내 맵과 타일 데이터를 **JSON 파일로 저장 및 비동기 로드**하는 기능을 제공합니다.  
맵의 빌딩 위치 데이터와 타일의 빌드 가능 여부를 포함하여, **Addressables와 Unity의 비동기 시스템을 활용해 효율적인 리소스 관리를 지원**합니다.  
또한, 에디터 모드에서의 초기화 및 저장 기능도 포함되어 있어 개발 및 테스트 편의성을 높였습니다.

### 🧱 코드

| Script명          | 설명                                                                |
|-------------------|-------------------------------------------------------------------|
|[ArrayBuildPos](../@Scripts/BuildMap/ArrayBuildPos.cs)| 빌딩 위치 데이터(ScriptableObject)를 JSON으로 저장, Addressables 비동기 로드 및 관리  |
|[ArrayMapPos](../@Scripts/BuildMap/ArrayMapPos.cs)| 타일 맵 데이터 저장/불러오기, 타일 빌드 가능 여부 관리                        |
|[BuildMap](../@Scripts/BuildMap/BuildMap.cs)| 저장된 빌딩 데이터 로드, 씬에 오브젝트 배치 및 NavMesh 갱신, Collider 일괄 제어      |
|[GridMap](../@Scripts/BuildMap/GridMap.cs)|타일 맵 초기화, 타일에 색상 및 건설 가능 여부 적용|


------
<br>

## 2. 카메라 및 드래그 시스템 (with DraggableObject)

이 기능은 유저가 씬에서 마우스로 드래그하여 오브젝트를 선택 및 조작하고,
카메라는 이동 가능하며, 각 오브젝트는 개별적으로 드래그에 반응하도록 구성되어 있습니다.

### 🧱 코드

| Script명          | 설명                                                                |
|-------------------|-------------------------------------------------------------------|
|[CameraController](../@Scripts/Contents/Framework/CameraController.cs)| [모바일,pc,웹] 모두 대응 가능한 구성으로 카메라를 이동+오브젝트 상호작용을 함,minLimit과 maxLimit으로 이동 영역 제한|
|[DragController](../@Scripts/BuildMap/Touch&Mouse/DragController.cs)| 마우스 클릭과 드래그 시작/종료 감지,LayerMask를 활용해 드래그 가능한 오브젝트 필터링,드래그한 범위 내의 오브젝트를 감지하여 DraggableObject에게 이벤트 전달|
|[DraggableObject](../@Scripts/BuildMap/Touch&Mouse/DraggableObject.cs)|실제로 드래그에 반응하는 오브젝트이며 해당 오브젝트 밑에 타일의 정보를 넘겨주고 DragController에 관리되어진다|

------
<br>

## 3. 건물 설치 시스템

BuildingPlacer는 건물 설치에 관련된 UI/프리뷰/연속 설치/롱프레스 드래그 및 NavMesh 갱신, 설치 취소, 저장까지 전반적인 설치 흐름을 관리하는 매니저입니다.

건물 선택 시 프리뷰 오브젝트를 생성하고, 그 위치에 맞춰 설치 가능한지 여부를 판단합니다.

연속 설치(도로 등)와 롱프레스 설치(기존 건물 교체)를 지원하며, 설치 시 NavMesh 갱신도 자동 수행합니다.

저장은 맵 좌표 및 타일 정보를 JSON 파일로 저장하여 게임 상태를 유지합니다.

플레이어의 골드를 사용하여 건설이 제한되며, 골드 부족 시 UI 알림도 포함됩니다.


## 🧱 코드

| Script명        | 설명                                                         |
| --------------- | ------------------------------------------------------------ |
| [BuildingPlacer](../@Scripts/BuildMap/BuildingPlacer.cs) | 게임의 출석, 오프라인 보상, 일일 초기화, 시간 기반 콘텐츠를 통합적으로 관리하는 시간 시스템 |

|  BuildingPlacer/건물 설치 관련 주요 함수      | 설명                                                         |
| --------------- | ------------------------------------------------------------ |
| [SelectBuildingType](../@Scripts/BuildMap/BuildingPlacer.cs#L107) | 건물 선택 시, 화면 중앙 기준으로 프리뷰 생성 및 설치 준비 |
| [AcceptBuild](../@Scripts/BuildMap/BuildingPlacer.cs#L452) | 한 개 건물 설치 확정, 실제 설치 및 데이터 저장 |
| [CancelBuild](../@Scripts/BuildMap/BuildingPlacer.cs#L540) | 설치 취소 처리, 임시 프리뷰 오브젝트 제거 및 상태 초기화 |
| [RemoveBuild](../@Scripts/BuildMap/BuildingPlacer.cs#L564) | 기존 건물 삭제, 데이터 제거 및 씬 오브젝트 정리 |

|  BuildingPlacer/도로 관련 주요 함수     | 설명                                                         |
| --------------- | ------------------------------------------------------------ |
| 도로 생성 ||
| [OnGroundTouched](../@Scripts/BuildMap/BuildingPlacer.cs#L208) | 연속 설치 모드에서 터치한 지점에 도로 프리뷰 생성 |
| [SaveandRemoveRoad](../@Scripts/BuildMap/BuildingPlacer.cs#L366) | 도로 프리뷰 위치 저장, 중복 방지 및 UI 업데이트 |
| [AcceptSequenceBuild](../@Scripts/BuildMap/BuildingPlacer.cs#L405) | 연속 건물 설치 확정, 여러 위치에 한꺼번에 설치 및 저장 |
| 도로 삭제 ||
| [OnGroundTouchedSecond](../@Scripts/BuildMap/BuildingPlacer.cs#L229) | 연속 삭제 모드에서 터치한 지점에 도로 삭제 프리뷰 생성 |
| [RemoveRoad](../@Scripts/BuildMap/BuildingPlacer.cs#L252) | 도로 삭제 처리, 해당 도로 오브젝트 및 데이터 제거 |



------
<br>

## 4. UIManager

게임 내 다양한 UI 요소(씬 UI, 팝업 UI, 토스트, 서브 아이템 등)를 일관성 있게 생성/관리하기 위해, 중앙 집중형 UI 관리 시스템이 필요했습니다. 이를 위해 `UIManager`를 도입하여, UI 계층 관리, 정렬 순서, 팝업 스택, Toast 메시지 등을 효율적으로 처리할 수 있도록 설계하였습니다.



## 🧱 코드

| Script명      | 설명                                                |
| ------------- | --------------------------------------------------- |
| [DayNightCycleManager](../@Scripts/DayNightCycle/DayNightCycleManager.cs) |하루(낮/밤) 주기를 관리하는 매니저,AmbientCycle,LightCycle,ParticleCycle을 관리함 |
| [AmbientCycle](../@Scripts/DayNightCycle/AmbientCycle.cs) | AmbientCycle주변 환경광(ambient light)의 밝기를 낮과 밤에 맞게 부드럽게 조절,DayNightCycleManager에서 시간 정보를 받아와 현재 낮인지 밤인지 판단 |
| [LightCycle](../@Scripts/DayNightCycle/LightCycle.cs) |Directional Light의 색상 및 밝기를 낮과 밤에 맞게 부드럽게 조절, DayNightCycleManager에서 시간 정보를 받아와 현재 낮인지 밤인지 판단 |
| [ParticleCycle](../@Scripts/DayNightCycle/ParticleCycle.cs) |하낮/밤 분위기에 맞춰 파티클(예: 반딧불, 먼지 등)의 활성/비활성 상태를 조절, DayNightCycleManager에서 시간 정보를 받아와 현재 낮인지 밤인지 판단 |

------
<br>
Assets/@Scripts/DayNightCycle/LightCycle.cs
Assets/@Scripts/DayNightCycle/ParticleCycle.cs

## 5. 툰셰이더
유니버설 렌더 파이프라인(URP)용 셀 셰이딩(툰 셰이딩) 기본 커스텀 셰이더

## 🧱 코드

| Script명      | 설명                                                |
| ------------- | --------------------------------------------------- |
| [ToonLightBase](../@Shader/ToonLightBase.shader) |기본 텍스처와 색상, 그림자, 스페큘러 하이라이트, 림라이트, 그리고 외곽선(아웃라인)을 포함하는 복합 조명 효과 제공 |

------
<br>


