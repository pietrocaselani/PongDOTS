using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public partial class PaddleMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        const float yBound = 2.25f;

        Entities.ForEach((ref Translation translation, in PaddleMovementData data) =>
        {
            translation.Value.y = math.clamp(translation.Value.y + (data.Speed * data.Direction * deltaTime), -yBound, yBound);
        }).Schedule();
    }
}