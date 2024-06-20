using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Services
{
    public class SpacialManager : MonoBehaviour, IService
    {
        private Grid _spacialGrid;
        public Grid SpacialGrid => _spacialGrid;
        public readonly Dictionary<Vector3Int, List<SpacialPartitionAgent>> partitionDict = new();
        private Vector3Int _playerPartition;
        public Vector3Int PlayerPartition => _playerPartition;

        public Tilemap PartitionTilemap;
        public readonly int XMax = 5;
        public readonly int YMax = 3;

        public readonly Vector3Int INVALID_PARTITION = new Vector3Int(-999, -999, -999);

        private void Awake()
        {
            _spacialGrid = GetComponent<Grid>();
            ServiceLocator.RegisterAsService(this);
        }

        public bool IsValidPartition(Vector3Int partition)
        {
            return PartitionTilemap.GetTile(partition) != null;
        }

        public Vector3 PartitionToWorld(Vector3Int partition)
        {
            return _spacialGrid.CellToWorld(partition);
        }

        public Vector3Int UpdatePartition(SpacialPartitionAgent obj)
        {
            var cell = _spacialGrid.WorldToCell(obj.transform.position);
            cell.z = 0;
            
            if(!IsValidPartition(cell))
            {
                Debug.LogWarning($"Object {obj} tried to move to invalid partition {cell}");
                return INVALID_PARTITION;
            }

            if(!partitionDict.ContainsKey(cell))
            {
                partitionDict.Add(cell, new List<SpacialPartitionAgent>() { obj });
                if (IsPlayer(obj))
                    _playerPartition = cell;
                return cell;
            }

            if (!partitionDict[cell].Contains(obj)) {
                //Try to remove this from its old partition
                if(partitionDict.TryGetValue(obj.Partition, out List<SpacialPartitionAgent> oldPartition))
                    oldPartition.Remove(obj);

                //Add this agent to the newly calculated partition
                if (IsPlayer(obj))
                    _playerPartition = cell;

                partitionDict[cell].Add(obj);
            }
            return cell;
        }

        public bool IsPlayer(SpacialPartitionAgent agent)
        {
            return agent.GetComponent<PlayerController>() != null;
        }
    }
}