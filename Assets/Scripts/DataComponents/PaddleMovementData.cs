using Unity.Entities;

[GenerateAuthoringComponent]
public struct PaddleMovementData : IComponentData
{
    public int Direction;
    public float Speed;
}