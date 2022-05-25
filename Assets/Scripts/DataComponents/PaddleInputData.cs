using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PaddleInputData : IComponentData
{
    public KeyCode UpKey;
    public KeyCode DownKey;
}