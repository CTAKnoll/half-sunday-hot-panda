using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Behavior
{

    public class MoveNearPlayer : BaseUnitBehavior
    {
        UnitNavigator nav;
        SpacialPartitionAgent agent;
        SpacialManager spacialManager;

        bool _foundPosition;

        private readonly Vector3Int[] offsets = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right,
        };

        public MoveNearPlayer(GameObject obj) : base(obj)
        {
            nav = obj.GetComponent<UnitNavigator>();
            agent = obj.GetComponent<SpacialPartitionAgent>();
            ServiceLocator.TryGetService(out spacialManager);
        }

        public override void Enter()
        {
            //Get the partition the player is in
            var playerPartition = spacialManager.PlayerPartition;

            // try to target a partition in a random direction adjacent to the player
            int index = (int) FloatExtensions.RandomBetween(0, offsets.Length);
            Vector3Int targetDirection = offsets[index];
            Vector3Int targetPartition = spacialManager.TryGetFreePartition(playerPartition + targetDirection);

            nav.SetDestination(targetPartition);
       }

        public override void Exit()
        {
            _foundPosition = false;
        }

        public override IUnitBehavior.Result Process(float deltaTime)
        {
            if (!_foundPosition)
                return IUnitBehavior.Result.DONE;

            if (!nav.IsMoving)
                return IUnitBehavior.Result.DONE;
            else
                return IUnitBehavior.Result.INCOMPLETE;
        }
    }

}