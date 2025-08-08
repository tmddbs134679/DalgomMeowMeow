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


ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ사용법
Assets/@Scripts/BuildMap/GridMap.cs
[BuildMap](../@Scripts/BuildMap/BuildMap.cs)

ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

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
| [BuildingPlacer](../@Scripts/BuildMap/BuildingPlacer.cs#L300) | 게임의 출석, 오프라인 보상, 일일 초기화, 시간 기반 콘텐츠를 통합적으로 관리하는 시간 시스템 |

|  BuildingPlacer 안에 함수 설명       | 설명                                                         |
| --------------- | ------------------------------------------------------------ |
| [BuildingPlacer](../@Scripts/BuildMap/BuildingPlacer.cs) | 게임의 출석, 오프라인 보상, 일일 초기화, 시간 기반 콘텐츠를 통합적으로 관리하는 시간 시스템 |

------
<br>

## 4. UIManager

게임 내 다양한 UI 요소(씬 UI, 팝업 UI, 토스트, 서브 아이템 등)를 일관성 있게 생성/관리하기 위해, 중앙 집중형 UI 관리 시스템이 필요했습니다. 이를 위해 `UIManager`를 도입하여, UI 계층 관리, 정렬 순서, 팝업 스택, Toast 메시지 등을 효율적으로 처리할 수 있도록 설계하였습니다.



## 🧱 코드

| Script명      | 설명                                                |
| ------------- | --------------------------------------------------- |
| [UIManager](../@Scripts/Managers/Core/UIManager.cs) | 씬UI, 팝업, 토스트까지 통합 관리하는 중앙 UI 시스템 |



------
<br>


## 5. 두투윈 활용 건물 텍스트 애니메이션, 버튼 애니메이션
각 건물의 기능을 직관적으로 보여주기 위해 애니메이션을 부여함으로써, 유저가 어떤 활동이 이루어지고 있는지 시각적으로 파악할 수 있도록 구현하였습니다. 

요리, 농사, 휴식, 놀이, 낚시 건물마다 각각 다른 방식의 텍스트 애니메이션이 동작합니다.



## 🧱 코드

| Script명               | 설명                                                         |
| ---------------------- | ------------------------------------------------------------ |
| [UI_TextAnimation](../@Scripts/UI/UI_TextAnimation.cs)   | `DOTween`을 활용하여 건물 종류에 따라 다양한 시각적 피드백 애니메이션을 텍스트로 표현하는 시스템 |
| [UI_ButtonAnimation](../@Scripts/UI/UI_ButtonAnimation.cs) | 버튼 클릭 시 크기 변화를 통해 사용자 피드백을 제공하는 직관적인 UI 애니메이션 구현 |


<img width="436" height="229" alt="건물 휮넉" src="https://github.com/user-attachments/assets/2499b26a-92a1-4581-81d3-e956c4a62d4b" />

<img width="436" height="229" alt="xx" src="https://github.com/user-attachments/assets/0d9b4cea-b181-4c51-aeaf-8f828645f60f" />

------
<br>

## 6. ObjectManager
게임 내 다양한 객체(캐릭터, 장비, 건물 등)를 일관되게 생성하고 초기화하며, 부모 객체와 위치를 동적으로 지정할 수 있는 통합된 오브젝트 관리 시스템이 필요했습니다. 코드 중복을 줄이고 유지보수를 용이하게 하기 위해 `ObjectManager`를 설계하였습니다.




## 🧱 코드

| Script명         | 설명                                                         |
| ---------------- | ------------------------------------------------------------ |
| [ObjectManger](../@Scripts/Managers/Core/ObjectManager.cs) | 다양한 게임 오브젝트를 동적으로 일관성 있게 생성하고 초기화하는 범용 오브젝트 생성 매니저 |

------
<br>

## 7. ResourceManager
다양한 게임 리소스(프리팹, 스프라이트 등)를 일관된 방식으로 로드하고, 

Addressables를 통한 비동기 로딩과 오브젝트 풀링을 결합하여 성능과 유지보수성을 향상시키기 위해 설계하였습니다.



## 🧱 코드

| Script명            | 설명                                                         |
| ------------------- | ------------------------------------------------------------ |
| [ResourceManager](../@Scripts/Managers/Core/ResourceManager.cs) | 동기/비동기 리소스 로딩과 오브젝트 풀링을 통합 관리하는 범용 리소스 매니저 |


------
<br>

## 8. Custom Editor (DebugManager, GameDataEditor, DataTransformer)
반복적인 디버깅 작업과 대량의 외부 데이터 적용 과정을 간소화하기 위해, Unity Editor 상에서 실행 가능한 **내부 툴링 시스템(Custom Editor)** 을 구축하였습니다. 이 시스템은 디자이너 및 개발자가 별도의 코드 수정 없이 데이터를 수정하거나 확인할 수 있도록 하며, 프로젝트 유지보수 효율성을 크게 향상시켰습니다



## 🧱 코드

| Script명            | 설명                                                         |
| ------------------- | ------------------------------------------------------------ |
| [DebugManager](../@Scripts/Managers/Edit/DebugManager.cs)    | 조건부 컴파일(UNITY_EDITOR, DEVELOPMENT_BUILD)로 필터링된 로그 출력 관리 도구 |
| [GameDataEditor](../@Scripts/Editor/GameDataEditor.cs)  | 에디터 창에서 저장된 게임 데이터를 직접 초기화하거나 골드 값 증가 등의 디버그 동작 수행 |
| [DataTransformer](../@Scripts/Editor/DataTransformer.cs) | Excel(.csv) 기반 게임 데이터들을 JSON으로 자동 변환 및 저장, JsonData 생성 자동화 |

<img width="300" height="268" alt="디버그" src="https://github.com/user-attachments/assets/74dd7812-e5ed-467f-b178-ad8209a5dc89" />
<img width="335" height="268" alt="게임데이터" src="https://github.com/user-attachments/assets/4f315242-18d7-4121-bb34-863e4c3de596" />
<img width="360" height="268" alt="파싱" src="https://github.com/user-attachments/assets/72c79886-1e6b-48c7-8fac-7564f065cff9" />

------
<br>

## 9. 그 외의 기능


## 🧱 코드

| Script명             | 설명                                                         |
| -------------------- | ------------------------------------------------------------ |
| [UI_PurchasePopup](../@Scripts/UI/Popup/UI_PurchasePopup.cs) | **수량 조절**과 **재화 환전**을 동시에 지원하는 구매 전용 팝업 UI |
| [BackgroundMove](../@Scripts/UI/Etc/BackgroundMove.cs)   | RawImage의 UV 좌표를 이동시켜 배경 이미지에 자연스러운 흐름을 주는 스크롤 효과 구현 클래스 |
| [SoundManager](../@Scripts/Managers/Core/SoundManager.cs)     | 사운드 타입별 AudioSource를 분리 관리하고, 리소스를 캐싱하여 효율적으로 재생하는 게임 전용 사운드 매니저 |

- Notify 기능 : 캐릭터, 장비, 퀘스트 뽑기 완료 시 알림 기능을 하는 Notify 기능 구현

<img width="280" height="169" alt="ㅊㅊ" src="https://github.com/user-attachments/assets/0fc7cc2e-c350-4664-901a-59043a38a9c5" />

<img width="350" height="200" alt="back" src="https://github.com/user-attachments/assets/f62b9bde-149d-4277-88ef-8a59d4b13c78" />
<img width="250" height="250" alt="노티" src="https://github.com/user-attachments/assets/c1462dcb-e1e5-4c28-8474-a165b18776fb" />

-----



