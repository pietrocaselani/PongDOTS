using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

[AlwaysSynchronizeSystem]
public partial class PlayerInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref PaddleMovementData movementData, in PaddleInputData inputData) =>
        {
            movementData.Direction = 0;

            movementData.Direction += Input.GetKey(inputData.UpKey) ? 1 : 0;
            movementData.Direction -= Input.GetKey(inputData.DownKey) ? 1 : 0;
        }).Run();
    }
}