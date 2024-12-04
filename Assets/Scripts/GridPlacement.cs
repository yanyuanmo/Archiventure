using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archiventure
{
    public class GridPlacement : MonoBehaviour
    {
        private Grid _grid;

        void Start() 
        {
            //_grid = Grid.FindObjectOfType<Grid>();
            _grid = Grid.FindFirstObjectByType<Grid>();
        }

        void Update()
        {
            Vector3Int _cp = _grid.LocalToCell(transform.localPosition);
            transform.localPosition = _grid.GetCellCenterLocal(_cp);
        }
    }
}
