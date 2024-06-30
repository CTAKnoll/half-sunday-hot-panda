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

        SpacialManager.Loan _loan;

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
            var queue = new Queue<Vector3Int>(offsets);

            while (queue.TryDequeue(out var offset))
            {
                if(spacialManager.TryBorrowPartition(offset + playerPartition, this, out SpacialManager.Loan loan))
                {
                    _foundPosition = true;
                    _loan = loan;
                    break;
                }
            }
            Vector3Int targetPartition = _loan.loanedPartition;
            Debug.Log($"Agent: {agent.name}, Before: {playerPartition} After: {targetPartition}");
            nav.SetDestination(targetPartition);
       }

        public override void Exit()
        {
            _loan.Release?.Invoke();
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