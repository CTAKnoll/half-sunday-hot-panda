using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            Vector3Int chosenPartition = Vector3Int.zero;
            //Check area around it for valid partitions
            foreach(var offset in offsets)
            {
                var partToCheck = playerPartition + offset;
                if(spacialManager.IsValidPartition(partToCheck))
                {
                    chosenPartition = partToCheck;
                    _foundPosition = true;
                    break;
                }    
            }

            nav.SetDestination(spacialManager.PartitionToWorld(chosenPartition));
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