## 1. 배틀 시스템

> 플레이어의 조작이 최소화된 실시간 자동 전투 시스템입니다.  
> 아군 캐릭터는 상태에 따라 이동, 공격, 복귀 등을 수행하며,  
> 적을 모두 처치하면 스테이지를 클리어하고 보상을 획득합니다.

- 아군 캐릭터는 NavMeshAgent를 사용해 적을 탐지하고 자동으로 전투에 참여합니다.
- 전투는 기본 공격 및 스킬 공격으로 이루어지며, 상태머신 기반의 팀 제어 로직을 포함합니다.
- 승리 또는 패배 시 해당 결과에 따라 카메라 연출과 UI가 전환되며, 스테이지 보상 처리도 함께 이루어집니다



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

> 각 캐릭터는 고유한 스킬을 가지고 있으며, 전투 중 하단의 버튼을 눌러 해당 스킬을 사용할 수 있습니다.  
- SkillSetting 클래스는 캐릭터의 SkillID를 기반으로 스킬 아이콘과 실행 버튼을 UI에 매핑해줍니다.  
- SkillLibrary는 스킬 번호에 따라 다양한 효과를 코루틴으로 실행합니다.  
- 실행되는 스킬에 따라 시각적인 연출이 필요할 경우, EffectHandler를 통해서 EffectManager에 접근하여 파티클 이펙트를 재생해줍니다.  




## 🧱 코드

| Script명          | 설명                                                         |
| ----------------- | ------------------------------------------------------------ |
| [SkillLibrary](../@Scripts/Contents/Battle/Skill/SkillLibrary.cs)   | 	스킬 번호와 실행 코루틴을 매핑하여 캐릭터가 스킬을 사용할 수 있도록 처리. 공격, 버프, 힐, 디버프 등 다양한 스킬 효과를 담당|
| [EffectManager](../@Scripts/Contents/Battle/Skill/Effect/EffectManager.cs)   | 	각 스킬에 맞는 파티클 프리팹을 생성하거나 재사용하여 위치, 색상, 지속시간 등을 제어. FireHand, Rain, Buff 등 다양한 이펙트를 코루틴으로 처리|








------
<br>

