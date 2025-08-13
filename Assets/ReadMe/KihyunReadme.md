## 1. 건물 기능 시스템



이 시스템은 **게임 내 다양한 건물을 통해 자원 생산, 보상 지급, 상호작용**이 이루어지도록 구현된 핵심 기능입니다.
플레이어는 각 건물(농사, 요리, 휴식, 놀이, 슬롯머신 등)에 접근하여 **해당 건물 고유의 기능을 실행**할 수 있습니다.
건물은 **업그레이드**를 통해 생산 속도, 보상량, 확률 등을 향상시킬 수 있으며,
일부 건물은 **자동 생산 또는 주기적인 보상 지급** 기능을 갖추어 플레이어가 직접 조작하지 않아도 게임이 진행되도록 설계되었습니다.

또한, 각 건물의 상태(대기, 생산 중, 보상 준비 완료)는 **UI와 애니메이션**으로 직관적으로 표현되며,
플레이어의 행동(건설, 생산 시작, 보상 수령)에 따라 즉시 반응하도록 구현되었습니다.

## 🧱 코드



| Script명                                                     | 설명                                                         |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| [BuildingBase](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 모든 건물의 공통 속성과 동작(상태 관리, 타이머, 생산 로직)을 정의한 기본 클래스 |
| [BuildingManager](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 씬 내 건물들을 관리하고, 생산·보상·상태 업데이트를 일괄 처리하는 매니저 |
| [UI_BuildPopup](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 건물 건설 및 업그레이드 UI, 콘텐츠 해금 여부에 따른 버튼 활성/비활성 관리 |
| [UI_BuildingProgress](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 건물 생산 진행 상황을 시각적으로 표시하는 UI 컴포넌트        |

이 시스템은 **플레이어의 진행 상황에 따라 새로운 콘텐츠(챕터, 건물 등)를 해금**하는 기능을 제공합니다.
퀘스트 완료, 누적 골드 달성 등 다양한 조건을 만족하면 해당 콘텐츠가 해금되며,
해금 가능 상태가 되면 **UI를 통해 알림**을 주어 플레이어가 쉽게 인지할 수 있도록 구현되었습니다.

해금 조건은 **여러 개를 동시에 설정**할 수 있으며, 모든 조건이 충족되어야 해금이 가능합니다.
조건이 만족되면 UI 버튼 활성화, 해금 완료 표시, 토스트 메시지 출력 등을 통해 피드백을 제공합니다.
또한, 해금 로직은 **중앙 관리형 매니저(QuestManager)**에서 일괄 처리되어,
UI(`UI_ChapterPopup`, `UI_BuildPopup`)와 데이터가 항상 동기화되도록 설계되었습니다.

## 🧱 코드



| Script명                                                     | 설명                                                         |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| [QuestManager](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 퀘스트 진행 상황과 콘텐츠 해금 조건을 관리하고, 조건 충족 시 해금 처리 및 UI 갱신 |
| [UI_ChapterPopup](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 챕터별 해금 조건 및 상태를 UI에 표시하고, 해금 가능 여부에 따라 버튼 활성/비활성 |
| [UI_BuildPopup](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 건물 해금 여부를 확인하여 건설 버튼 상태를 변경하고, 해금 시 건물 건설 가능 처리 |
| [UnlockCondition](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 각 콘텐츠의 해금 조건(퀘스트, 누적 골드 등)을 데이터 구조로 정의 |

------

## 3. 튜토리얼 시스템



이 시스템은 **게임 초반 플레이어에게 필요한 조작 방법과 진행 흐름을 안내**하는 기능을 제공합니다.
튜토리얼은 **단계별 시나리오**로 구성되어 있으며, 각 단계에서 플레이어가 수행해야 할 작업과
해당 UI 요소를 명확하게 인지할 수 있도록 **하이라이트와 딤 처리**를 제공합니다.

단계 진행은 조건 충족 시 자동으로 다음 단계로 넘어가며,
필요할 경우 특정 UI 요소 외의 상호작용을 차단하여 **집중도 있는 안내**가 가능합니다.
튜토리얼 진행 상황은 저장되어, 이미 완료한 경우 다시 실행되지 않도록 처리했습니다.

## 🧱 코드



| Script명                                                     | 설명                                                         |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| [TutorialManager](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 튜토리얼 전체 진행 관리, 단계 전환, 하이라이트/딤 처리, 상호작용 제한 로직 담당 |
| [TutorialStep](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 각 튜토리얼 단계의 데이터(설명, 하이라이트 대상, 시작/완료 이벤트) 정의 |
| [Highlighter](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 지정된 UI 요소를 시각적으로 강조하는 하이라이트 효과 구현    |
| [UI_Tutorial](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 튜토리얼 설명 텍스트 및 이미지 출력, 단계별 UI 표시 관리     |

------

## 4. 그외 기능



이 시스템은 **플레이어가 슬롯머신 건물을 통해 랜덤 보상을 획득**할 수 있도록 구현된 미니게임입니다.
슬롯은 **DOTween 애니메이션**을 활용하여 회전하며, 각 슬롯은 서로 다른 시간에 멈추도록 설정되어
현실적인 슬롯머신의 움직임과 긴장감을 제공합니다.

결과는 **가중치 기반 확률 시스템**으로 산출되며, 회전이 종료된 슬롯의 중앙 아이콘 스프라이트를
실제 결과와 매칭하여 UI와 로직의 일관성을 유지합니다.
아이콘은 순환 구조로 재배치되어, 애니메이션 반복 시 끊김 없는 회전 효과가 구현됩니다.

최종 결과에 따라 보상을 지급하며, 효과음과 시각 효과를 함께 재생하여 몰입감을 높였습니다.

## 🧱 코드



| Script명                                                     | 설명                                                         |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| [SlotMachineBuilding](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 슬롯머신 건물의 회전, 결과 산출, 보상 지급, 애니메이션 제어를 담당 |
| [UI_SlotMachinePopup](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 슬롯머신 UI 인터페이스 관리, 시작 버튼, 결과 표시, 보상 수령 처리 |
| [SlotResult](https://github.com/tmddbs134679/DalgomMeowMeow/blob/main/Assets/ReadMe) | 슬롯 결과 데이터 구조(아이콘, 보상 금액, 확률 가중치 등) 정의 |






