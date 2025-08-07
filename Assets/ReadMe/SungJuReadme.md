## 1. 배틀 시스템



B



## 🧱 코드

| Script명          | 설명                                                         |
| ----------------- | ------------------------------------------------------------ |
| [StageDataManager](../@Scripts/Data/Battle/StageData/StageDataManager.cs) |  플레이어 캐릭터 데이터와 스테이지 정보를 관리하며, 씬 간 데이터 전달, 스테이지 저장 및 보상 지급 처리|
| [BattleStageManager](../@Scripts/Contents/Battle/Stage/BattleStageManager.cs)   | 	현재 스테이지 정보를 기반으로 적 몬스터를 랜덤으로 생성하고, 각 적을 지정된 부모 오브젝트에 배치|
| [BattleManager](../@Scripts/Managers/Contents/BattleManager.cs) | 	전투 중 적/아군 수를 체크하여 승패를 판단하고, 승리 시 보상 지급 및 카메라 연출/타이틀 이동 처리|
| [TeamController](../@Scripts/Contents/Battle/Character/TeamController.cs)   | 	아군 팀의 상태머신 (Moving, Fighting, Returning)을 기반으로 팀 전체 이동 및 복귀 로직 제어 |
| [TeamManager](../@Scripts/Contents/Battle/Character/TeamManager.cs)   | 	StageDataManager에서 전달된 아군 캐릭터 데이터를 바탕으로 캐릭터를 초기화하고 프리팹(모델) 로드 및 설정 |
| [BattleCharacter](../@Scripts/Contents/Battle/Character/BattleCharacter.cs)   | 전투 캐릭터의 베이스 클래스. 체력, 이동, 공격, 피격, 타겟 탐색, 스킬 사용, 사망 처리 등 전투 로직을 담당|





------
<br>

## 2. 스킬 시스템
