using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Services
{
    public class SpacialManager : MonoBehaviour, IService
    {
        private Grid _spacialGrid;
        public Grid SpacialGrid => _spacialGrid;
        public Dictionary<Vector3Int, SpacialPartitionAgent> OccupiedPartitions;

        private Vector3Int _playerPartition;
        public Vector3Int PlayerPartition => _playerPartition;

        public Tilemap PartitionTilemap;
        public readonly int XMax = 5;
        public readonly int YMax = 3;

        public readonly Vector3Int INVALID_PARTITION = new Vector3Int(-999, -999, -999);

        private void Awake()
        {
            _spacialGrid = GetComponent<Grid>();
            OccupiedPartitions = new();
            ServiceLocator.RegisterAsService(this);
        }

        public bool IsValidPartition(Vector3Int partition)
        {
            return PartitionTilemap.GetTile(partition) != null;
        }

        public bool IsOccupiedPartition(Vector3Int partition)
        {
            return OccupiedPartitions.ContainsKey(partition);
        }

        public Vector3 PartitionToWorld(Vector3Int partition)
        {
            return _spacialGrid.CellToWorld(partition);
        }

        public Vector3Int UpdateDestinationPartition(SpacialPartitionAgent obj, Vector3Int target)
        {
            if (target == obj.Partition)
                return target; // nothing to do;

            if(!IsValidPartition(target))
            {
                Debug.LogWarning($"Object {obj} tried to move to invalid partition {target}");
                return INVALID_PARTITION;
            }
            
            if (OccupiedPartitions.ContainsValue(obj))
            {
                OccupiedPartitions.Remove(obj.Partition);
            }
            
            //Add this agent to the newly calculated partition
            if (IsPlayer(obj))
                _playerPartition = target;

            OccupiedPartitions.Add(target, obj);
            return target;
        }

        public Vector3Int TryGetFreePartition(Vector3Int cell)
        {
            Queue<Vector3Int> queue = new Queue<Vector3Int>();
            queue.Enqueue(cell);
            while (queue.Any())
            {
                Vector3Int candidate = queue.Dequeue();
                if (CheckValidAndFree(candidate, queue))
                    return candidate;

                if (queue.Count >= 15)
                {
                    Debug.LogError("Early out DEBUG");
                    return INVALID_PARTITION;
                }
            }
            return INVALID_PARTITION;
        }

        private bool CheckValidAndFree(Vector3Int cell, Queue<Vector3Int> queue)
        {
            if (!IsValidPartition(cell))
                return false;
            if (!IsOccupiedPartition(cell))
                return true;
            queue.Enqueue(new Vector3Int(cell.x + 1, cell.y));
            queue.Enqueue(new Vector3Int(cell.x - 1, cell.y));
            queue.Enqueue(new Vector3Int(cell.x, cell.y + 1));
            queue.Enqueue(new Vector3Int(cell.x, cell.y - 1));
            return false;
        }

        public bool IsPlayer(SpacialPartitionAgent agent)
        {
            return agent.GetComponent<PlayerController>() != null;
        }
    }
}