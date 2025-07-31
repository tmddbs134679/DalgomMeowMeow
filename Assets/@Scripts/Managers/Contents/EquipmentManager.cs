using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.TextCore.Text;
using System;

public class EquipmentManager 
{


    public Action EquipInfoChanged;

    #region Equipment

    public void EquipCharacterVisual(AICharacter ai, Character character, Equipment previewEquipment = null)
    {
        foreach (var equipUid in character.EquippedItemIds)
        {
            Equipment equip = Managers.Game.OwnedEquipments.Find(e => e.UniqueId == equipUid);
            if (equip == null) continue;
            AttachEquipmentToCharacter(ai, equip);
        }

        if (previewEquipment != null)
        {
            AttachPreviewToCharacter(ai, previewEquipment);
        }
    }

    public void EquipItem(Character character, Equipment equipment)
    {
        if (character == null || equipment == null)
            return;

        var uniqueId = character.UniqueId;

        var targetCharacter = Managers.Game.CharacterMap.ContainsKey(uniqueId)
        ? Managers.Game.CharacterMap[uniqueId]
            : Managers.Game.Characters.Find(c => c.UniqueId == uniqueId);
        if (targetCharacter == null)
            return;

        var type = equipment.EquipmentData.EquipmentType;

        if (!string.IsNullOrEmpty(equipment.EquippedByCharacterId) &&
            equipment.EquippedByCharacterId != targetCharacter.UniqueId)
        {
            var previousOwner = Managers.Game.Characters.Find(c => c.UniqueId == equipment.EquippedByCharacterId);
            if (previousOwner != null)
                UnEquipItem(previousOwner, equipment);
        }

        if (targetCharacter.EquippedItems.TryGetValue(type, out var oldEquipment))
            UnEquipItem(targetCharacter, oldEquipment);

        targetCharacter.EquippedItems[type] = equipment;
        if (!targetCharacter.EquippedItemIds.Contains(equipment.UniqueId))
            targetCharacter.EquippedItemIds.Add(equipment.UniqueId);

        equipment.EquippedByCharacterId = targetCharacter.UniqueId;
        equipment.IsEquipped = true;
        equipment.IsConfirmed = true;

        if (Managers.Game.CharacterInMainScene.TryGetValue(uniqueId, out var ai))
            AttachEquipmentToCharacter(ai, equipment);

        EquipInfoChanged?.Invoke();
        Managers.Game.OnCharacterChanged?.Invoke();
        Managers.Game.SaveGame();
    }

    public void UnEquipItem(Character character, Equipment equipment)
    {
        var uniqueId = character.UniqueId;

        var targetCharacter = Managers.Game.CharacterMap.ContainsKey(uniqueId)
        ? Managers.Game.CharacterMap[uniqueId]
            : Managers.Game.Characters.Find(c => c.UniqueId == uniqueId);
        if (targetCharacter == null) return;

        var type = equipment.EquipmentData.EquipmentType;

        targetCharacter.EquippedItems.Remove(type);
        targetCharacter.EquippedItemIds.Remove(equipment.UniqueId);

        equipment.EquippedByCharacterId = null;
        equipment.IsEquipped = false;

        if (Managers.Game.CharacterInMainScene.TryGetValue(uniqueId, out var ai))
            DetachEquipmentFromCharacter(ai, type);

        EquipInfoChanged?.Invoke();
        Managers.Game.OnCharacterChanged?.Invoke();
        Managers.Game.SaveGame();
    }


    public void SetInitEquipment(AICharacter character)
    {
        var equippedIdsCopy = new List<string>(character.Stat.data.EquippedItemIds);

        foreach (var equipUid in equippedIdsCopy)
        {
            Equipment equip = Managers.Game.OwnedEquipments.Find(e => e.UniqueId == equipUid);
            if (equip == null)
            {
                Debug.LogWarning($"장착 장비 UID {equipUid} 를 못 찾음");
                continue;
            }

            if (!character.Stat.data.EquippedItems.ContainsKey(equip.EquipmentData.EquipmentType))
                character.Stat.data.EquippedItems.Add(equip.EquipmentData.EquipmentType, equip);

            EquipItem(character.Stat.data, equip);
        }
    }

    public void ApplyEquipmentPreview(AICharacter replica, Character character)
    {
        if (replica == null || character == null)
            return;

        // 기존 장비 제거
        foreach (var kvp in replica.equipmentBones)
        {
            foreach (Transform child in kvp.Value)
                Managers.Resource.Destroy(child.gameObject);
        }

        // 캐릭터 장비 복제해서 장착
        foreach (var pair in character.EquippedItems)
        {
            var equipment = pair.Value;

            if (equipment == null)
                continue;

            AttachEquipmentToCharacter(replica, equipment);
        }
    }

    private void AttachPreviewToCharacter(AICharacter ai, Equipment equipment)
    {
        var type = equipment.EquipmentData.EquipmentType;

        if (!ai.equipmentBones.TryGetValue(type, out var bone))
        {
            Debug.LogWarning($"[AttachPreviewToCharacter] 장비 본이 존재하지 않음: {type}");
            return;
        }

        // 기존 미리보기 장비 제거 (Equipped_로 시작하는 기존 시각화 삭제)
        foreach (Transform child in bone)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        // 새 장비 시각화
        EquipmentController preview = Managers.Object.Spawn<EquipmentController>(
            Vector3.zero, equipment.EquipmentData.DataId, bone);
    }
    private void AttachEquipmentToCharacter(AICharacter ai, Equipment equipment)
    {
        var type = equipment.EquipmentData.EquipmentType;

        if (!ai.equipmentBones.TryGetValue(type, out var bone))
        {
            Debug.LogWarning($"장비 본이 존재하지 않음: {type}");
            return;
        }

        foreach (Transform child in bone)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        EquipmentController go = Managers.Object.Spawn<EquipmentController>(Vector3.zero, equipment.EquipmentData.DataId, bone);

    }

    private void DetachEquipmentFromCharacter(AICharacter ai, EEquipmentType type)
    {
        if (!ai.equipmentBones.TryGetValue(type, out var bone))
        {
            Debug.LogWarning($"장비 본이 없음 : {type}");
            return;
        }

        foreach (Transform child in bone)
        {
            Managers.Resource.Destroy(child.gameObject);
        }
    }

    public Equipment AddEquipment(string key)
    {
        if (key.Equals("None"))
            return null;

        Equipment equip = new Equipment(key);
        equip.IsConfirmed = false;

        Managers.Game.OwnedEquipments.Add(equip);
        EquipInfoChanged?.Invoke();

        return equip;
    }


    #endregion
}
