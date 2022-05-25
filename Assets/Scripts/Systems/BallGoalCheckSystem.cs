using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public partial class BallGoalCheckSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        var xBound = 6f;

        var player1Score = 0;
        var player2Score = 0;
        var playerScored = 0;

        Entities
            .WithAll<BallTag>()
            .ForEach((Entity entity, in Translation translation) =>
            {
                var position = translation.Value;

                if (position.x >= xBound)
                {
                    //gameStateData.player1Score++;
                    player1Score++;
                    playerScored = 1;

                    Debug.Log(">>> Player 1 scored");
                    ecb.DestroyEntity(entity);
                }
                else if (position.x <= -xBound)
                {
                    //gameStateData.player2Score++;
                    player2Score++;
                    playerScored = 2;

                    Debug.Log(">>> Player 2 scored");
                    ecb.DestroyEntity(entity);
                }
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();

        if (playerScored != 0)
            GameManager.Score(playerScored);
    }
}