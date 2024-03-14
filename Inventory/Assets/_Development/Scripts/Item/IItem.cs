using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public ItemData Data { get; }
    public void Init(ItemData data, Sprite sprite);
    public void UpdateBoxCollider();
}
