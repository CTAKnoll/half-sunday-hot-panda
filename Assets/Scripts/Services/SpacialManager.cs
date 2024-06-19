using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class SpacialManager : MonoBehaviour, IService
    {
        private Grid _spacialGrid;
        public Grid SpacialGrid => _spacialGrid;
        public readonly Dictionary<Vector3Int, List<SpacialPartitionAgent>> _partitionDict = new();

        public readonly int XMax = 5;
        public readonly int YMax = 3;

        public readonly Vector3Int INVALID_PARTITION = new Vector3Int(-999, -999, -999);

        private void Awake()
        {
            _spacialGrid = GetComponent<Grid>();
            ServiceLocator.RegisterAsService(this);
        }

        public Vector3 PartitionToWorld(Vector3Int partition)
        {
            return _spacialGrid.CellToWorld(partition);
        }

        public Vector3Int UpdatePartition(SpacialPartitionAgent obj)
        {
            var cell = _spacialGrid.WorldToCell(obj.transform.position);
            cell.z = 0;
            
            if(Mathf.Abs(cell.x) > XMax || Mathf.Abs(cell.y) > YMax)
            {
                return INVALID_PARTITION;
            }

            if(!_partitionDict.ContainsKey(cell))
            {
                _partitionDict.Add(cell, new List<SpacialPartitionAgent>() { obj });
                return cell;
            }

            if (!_partitionDict[cell].Contains(obj)) {
                //Try to remove this from its old partition
                if(_partitionDict.TryGetValue(obj.Partition, out List<SpacialPartitionAgent> oldPartition))
                    oldPartition.Remove(obj);

                //Add this agent to the newly calculated partition
                _partitionDict[cell].Add(obj);
            }
            return cell;
        }

    }
}