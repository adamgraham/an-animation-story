using System.Collections.Generic;

namespace Zigurous.Terrain.CubeTerrain
{
    internal class CubeTerrainCoordinateGrid
    {
        private Dictionary<Coordinates2D, CubeTerrainCoordinateGridData> _grid;

        public CubeTerrainCoordinateGrid()
        {
            _grid = new Dictionary<Coordinates2D, CubeTerrainCoordinateGridData>();
        }

        public CubeTerrainCoordinateGridData this[Coordinates2D coordinates]
        {
            get {
                return _grid.ContainsKey(coordinates) ? _grid[coordinates] : null;
            }

            internal set {
                _grid[coordinates] = value;
            }
        }

        public CubeTerrainCoordinateGridData DataLeftOf(Coordinates2D coordinates)
        {
            return this[new Coordinates2D(coordinates.x - 1, coordinates.y)];
        }

        public CubeTerrainCoordinateGridData DataRightOf(Coordinates2D coordinates)
        {
            return this[new Coordinates2D(coordinates.x + 1, coordinates.y)];
        }

        public CubeTerrainCoordinateGridData DataTopOf(Coordinates2D coordinates)
        {
            return this[new Coordinates2D(coordinates.x, coordinates.y + 1)];
        }

        public CubeTerrainCoordinateGridData DataBottomOf(Coordinates2D coordinates)
        {
            return this[new Coordinates2D(coordinates.x, coordinates.y - 1)];
        }

    }

}
