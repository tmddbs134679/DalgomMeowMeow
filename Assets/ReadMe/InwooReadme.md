## 1. 맵 및 타일 저장/불러오기 

이 기능은 게임 내 맵과 타일 데이터를 **JSON 파일로 저장 및 비동기 로드**하는 기능을 제공합니다.  
맵의 빌딩 위치 데이터와 타일의 빌드 가능 여부를 포함하여, **Addressables와 Unity의 비동기 시스템을 활용해 효율적인 리소스 관리를 지원**합니다.  
또한, 에디터 모드에서의 초기화 및 저장 기능도 포함되어 있어 개발 및 테스트 편의성을 높였습니다.


### 🧱 코드

| Script명          | 설명                                                                |
|-------------------|-------------------------------------------------------------------|
|[ArrayBuildPos](../@Scripts/BuildMap/ArrayBuildPos.cs)| 빌딩 위치 데이터(ScriptableObject)를 JSON으로 저장, Addressables 비동기 로드 및 관리  |
| ArrayMapPos.cs     | 타일 맵 데이터 저장/불러오기, 타일 빌드 가능 여부 관리                        |
| BuildData.cs       | 빌딩 데이터 구조체                                                    |
| MapSaveData.cs     | 저장 시 빌딩 데이터 리스트를 포함하는 JSON 직렬화용 클래스                       |
| TileRow.cs, TileData.cs | 타일 행 및 개별 타일 데이터 구조체                                        |
| MapTileSaveData.cs | 타일 맵 저장/로드에 사용되는 JSON 직렬화용 클래스   

[ArrayBuildPos](../@Scripts/BuildMap/ArrayBuildPos.cs)
## 🧱 코드
[](../)  Assets/@Scripts/BuildMap/ArrayBuildPos.cs
| Script명          | 설명                                                         |
| ----------------- | ------------------------------------------------------------ |
| [FoodManager](../@Scripts/Managers/Contents/FoodManager.cs)   | **음식 리스트 관리**와 **음식 판매 시스템**을 담당하며, `Enqueue`를 통해 **음식을 추가**하고, **자동 판매** 기능을 제공 |
| [IngredientSet](../@Scripts/Contents/Building/CookingBuilding.cs) | **음식 생산에 사용되는 재료**와 그 **밭 건물 레벨**을 관리하며, **평균 밭 레벨**에 따라 **가격 보정 계수**를 계산하고 재료를 추가하거나 초기화하는 기능 |


<img width="1571" height="188" alt="음식" src="https://github.com/user-attachments/assets/840e42f5-d3c4-43f5-80a8-a45138c41efa" />


------
<br>


## 2. 캐릭터 장비 관리 시스템 (미리보기)

이 시스템은 **캐릭터 장비**를 관리하는 핵심 기능으로, **장비 착용, 장비 미리보기, 장비 탈착**을 처리합니다.
 특히 **미리보기 기능**을 통해 플레이어는 장비를 장착하기 전에 **캐릭터 모델에 적용된 장비**를 실시간으로 확인할 수 있습니다.
 이 기능은 게임의 **UI/UX**를 향상시키며, **장비 선택**에 대한 직관적인 경험을 제공합니다.





## 🧱 코드

| Script명             | 설명                                                         |
| -------------------- | ------------------------------------------------------------ |
| [EquipmentManager](../@Scripts/Managers/Contents/EquipmentManager.cs) | 캐릭터의 **장비 착용, 미리보기, 탈착**을 관리하며, **장비의 외형 변화**를 실시간으로 반영하고 **UI/UX**를 향상시키는 시스템 |



<img width="800" height="400" alt="장비" src="https://github.com/user-attachments/assets/a9f3572a-af5f-442f-9016-770a0c19c83f" />



------
<br>

## 3. 출석체크 & 오프라인 보상 시스템 & 여행 시스템

게임 내 시간 기반 컨텐츠(출석 보상, 오프라인 보상, 일일 초기화, 여행 시간 등)를 일관되게 관리하기 위해 **중앙 집중형 시간 관리 매니저**가 필요했습니다.
 `TimeManager`는 플레이어의 로그인 시간, 종료 시간, 보상 수령 시간 등을 추적하고, 다양한 타임 이벤트를 처리하는 역할을 합니다.




## 🧱 코드

| Script명        | 설명                                                         |
| --------------- | ------------------------------------------------------------ |
| [TimeManager](../@Scripts/Managers/Contents/TimeManager.cs) | 게임의 출석, 오프라인 보상, 일일 초기화, 시간 기반 콘텐츠를 통합적으로 관리하는 시간 시스템 |

<img width="320" height="300" alt="출석체크" src="https://github.com/user-attachments/assets/a4904684-1704-4dea-9423-8922c19d2b4b" />
<img width="350" height="300" alt="오프라인" src="https://github.com/user-attachments/assets/e5ab416f-0413-4d48-a7aa-cd2052e75f7d" />
<img width="310" height="300" alt="여행" src="https://github.com/user-attachments/assets/fc820639-42f0-4b52-96a6-ed44f202a7ba" />

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

