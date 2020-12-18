using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zigurous.Terrain.CubeTerrain
{
    [RequireComponent(typeof(CubeTerrainBlockGenerator))]
    [DisallowMultipleComponent]
    public sealed class CubeTerrain : MonoBehaviour
    {
        [Header("References")]

        public Transform player;

        public Coordinates2D playerCoordinates
        {
            get
            {
                int playerX = this.player != null ? Mathf.RoundToInt(this.player.position.x) : 0;
                int playerY = this.player != null ? Mathf.RoundToInt(this.player.position.z) : 0;

                return new Coordinates2D(playerX, playerY);
            }
        }

        [Header("Terrain Settings")]

        public uint amountQuadrants = 64;
        public uint maxActiveQuadrants = 64;

        public uint maxWidth = 1024; // x-size-total
        public uint maxLength = 1204; // z-size-total

        public float updateDelay = 1.0f;

        public bool isRegenerative;
        public bool isInitialized { get; private set; }

        private List<CubeTerrainQuadrant> _quadrants;
        private CubeTerrainLayoutData _terrainLayout;

        internal CubeTerrainCoordinateGrid world;
        internal CubeTerrainBlockGenerator blockGenerator;
        internal int seed;

        [Header("Quadrant Settings")]

        public uint width = 16; // x-size
        public uint length = 16; // z-size
        public uint height = 16; // y-size

        public int seaLevel = 0; // y-baseline
        public int altitude = 0; // y-offset
        public int peak = 8; // y-clamp-max
        public int valley = -8; // y-clamp-min

        private void OnDestroy()
        {
            this.player = null;
            this.blockGenerator = null;
            this.world = null;

            _quadrants = null;
        }

        private void Awake()
        {
            // initialize/assign world properties
            this.blockGenerator = GetComponent<CubeTerrainBlockGenerator>();
            this.world = new CubeTerrainCoordinateGrid();

            // set random seed
            int permutations = 2048 * 2048;
            this.seed = Random.Range(-permutations / 2, permutations / 2);
        }

        private void Start()
        {
            StartCoroutine(InitializeTerrain());
        }

        private IEnumerator InitializeTerrain()
        {
            yield return InitializeWorldData();
            yield return InitializeBoundaries();
            yield return InitializeQuadrants();

            this.isInitialized = true;

            StartCoroutine(UpdateTerrain());
        }

        private IEnumerator InitializeWorldData()
        {
            Coordinates2D localCoordinates = Coordinates2D.zero;
            Coordinates3D worldCoordinates = Coordinates3D.zero;

            // fix the max size in case of rounding errors
            this.maxWidth = (this.maxWidth / this.width) * this.width;
            this.maxLength = (this.maxLength / this.length) * this.length;
            this.amountQuadrants = (uint)Mathf.Min(this.amountQuadrants, (this.maxWidth * this.maxLength) / (this.width * this.length));

            // set coordinate bounds
            int startX = -(int)this.maxWidth / 2;
            int startY = -(int)this.maxLength / 2;
            int endX = Mathf.Abs(startX);
            int endY = Mathf.Abs(startY);

            // create world data for every coordinate
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    // set local coordinates
                    localCoordinates.x = x;
                    localCoordinates.y = y;
                    
                    // set world coordinates
                    worldCoordinates.x = x;
                    worldCoordinates.y = CalculateYPositionAt(localCoordinates);
                    worldCoordinates.z = y;

                    // set world data
                    CubeTerrainCoordinateGridData data = new CubeTerrainCoordinateGridData();
                    data.worldCoordinates = worldCoordinates;
                    data.edgeDistance = CalculateEdgeDistanceFrom(localCoordinates);
                    this.world[localCoordinates] = data;
                }
            }

            _terrainLayout = new CubeTerrainLayoutData(terrain: this);

            yield break;
        }

        private int CalculateYPositionAt(Coordinates2D coordinates)
        {
            // calculate y-position using perlin noise
            float perlinX = (float)(coordinates.x + this.seed) / this.width;
            float perlinY = (float)(coordinates.y + this.seed) / this.length;
            float perlin = this.altitude + (Mathf.PerlinNoise(perlinX, perlinY) * this.height + 1);
            return Mathf.Clamp(Mathf.FloorToInt(perlin), this.valley, this.peak);
        }

        private int CalculateEdgeDistanceFrom(Coordinates2D coordinates)
        {
            if (IsTerrainFlat()) { return int.MaxValue; }

            int height = CalculateYPositionAt(coordinates);
            int length = (int)Mathf.Sqrt(this.maxWidth * this.maxLength) / 2;

            Coordinates2D neighborCoordinates = coordinates;

            for (int i = 1; i <= length; i++)
            {
                for (int j = -i; j <= i; j++)
                {
                    for (int k = -i; k <= i; k++)
                    {
                        neighborCoordinates.x = j + coordinates.x;
                        neighborCoordinates.y = k + coordinates.y;

                        int y = CalculateYPositionAt(neighborCoordinates);
                        if (y != height) {
                            return (int)neighborCoordinates.DistanceTo(coordinates) - 1;
                        }
                    }
                }
            }

            return int.MaxValue;
        }

        private bool IsTerrainFlat()
        {
            float perlinMin = this.altitude + (0.0f * this.height + 1);
            float perlinMax = this.altitude + (1.0f * this.height + 1);
            return Mathf.Clamp(perlinMin, this.valley, this.peak) == 
                   Mathf.Clamp(perlinMax, this.valley, this.peak);
        }

        private IEnumerator InitializeBoundaries()
        {
            // calculate boundaries
            float size = this.maxWidth + this.maxLength;
            float height = Mathf.Max(this.height, Mathf.Max(this.width, this.length)) * 2.0f;
            float thickness = Mathf.Max(this.maxWidth, this.maxLength);
            float offset = thickness / 2.0f;

            // create left boundary (-x)
            GameObject boundaryLeft = new GameObject("Boundary: Left");
            boundaryLeft.transform.parent = this.transform;
            boundaryLeft.transform.position = new Vector3(-(this.maxWidth / 2.0f) - offset - 0.5f, this.altitude, 0.0f);
            boundaryLeft.transform.localScale = new Vector3(thickness, height, size);
            boundaryLeft.AddComponent<BoxCollider>();
            yield return new WaitForEndOfFrame();

            // create right boundary (+x)
            GameObject boundaryRight = new GameObject("Boundary: Right");
            boundaryRight.transform.parent = this.transform;
            boundaryRight.transform.position = new Vector3((this.maxWidth / 2.0f) + offset - 0.5f, this.altitude, 0.0f);
            boundaryRight.transform.localScale = new Vector3(thickness, height, size);
            boundaryRight.AddComponent<BoxCollider>();
            yield return new WaitForEndOfFrame();

            // create bottom boundary (-z)
            GameObject boundaryBottom = new GameObject("Boundary: Bottom");
            boundaryBottom.transform.parent = this.transform;
            boundaryBottom.transform.position = new Vector3(0.0f, this.altitude, -(this.maxLength / 2.0f) - offset - 0.5f);
            boundaryBottom.transform.localScale = new Vector3(size, height, thickness);
            boundaryBottom.AddComponent<BoxCollider>();
            yield return new WaitForEndOfFrame();

            // create top boundary (+z)
            GameObject boundaryTop = new GameObject("Boundary: Top");
            boundaryTop.transform.parent = this.transform;
            boundaryTop.transform.position = new Vector3(0.0f, this.altitude, (this.maxLength / 2.0f) + offset - 0.5f);
            boundaryTop.transform.localScale = new Vector3(size, height, thickness);
            boundaryTop.AddComponent<BoxCollider>();
            yield return new WaitForEndOfFrame();

            // create floor boundary (+-y)
            GameObject floor = new GameObject("Boundary: Floor");
            floor.transform.parent = this.transform;
            floor.transform.position = new Vector3(0.0f, this.valley - 1, 0.0f);
            floor.transform.localScale = new Vector3(this.maxWidth * 2.0f, 1.0f, this.maxLength * 2.0f);
            floor.AddComponent<BoxCollider>();
            yield break;
        }

        private IEnumerator InitializeQuadrants()
        {
            _quadrants = new List<CubeTerrainQuadrant>();

            Coordinates2D[] coordinates = NearestQuadrantCoordinates(amount: (int)this.amountQuadrants);

            for (int i = 0; i < coordinates.Length; i++)
            {
                CubeTerrainQuadrant quadrant = new CubeTerrainQuadrant(this);
                quadrant.SetNeedsRepositionAt(coordinates[i]);
                quadrant.RepositionIfNeeded();

                _quadrants.Add(quadrant);

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator UpdateTerrain()
        {
            while (this != null)
            {
                UpdateQuadrants();
                yield return new WaitForSeconds(this.updateDelay);
            }
        }

        private void UpdateQuadrants()
        {
            CubeTerrainQuadrant[] quadrants = _quadrants.ToArray();
            Coordinates2D[] currentCoordinates = quadrants.Select(quadrant => quadrant.coordinates).ToArray();
            Coordinates2D[] nearestCoordinates = NearestQuadrantCoordinates(amount: quadrants.Length);
            Queue<Coordinates2D> nearestAvailableCoordinates = new Queue<Coordinates2D>(nearestCoordinates.Where(n => !currentCoordinates.Contains(n)));

            // reposition any distant quadrants, if necessary
            quadrants.ForEach(quadrant => {
                if (nearestAvailableCoordinates.Count == 0) { return; }
                // reposition the quadrant if it is not contained in the set of nearest quadrants
                if (!nearestCoordinates.Contains(quadrant.coordinates)) {
                    quadrant.SetNeedsRepositionAt(nearestAvailableCoordinates.Dequeue());
                }
            });

            // order the quadrants by the distance to the player
            Coordinates2D playerCoordinates = this.playerCoordinates;
            quadrants = quadrants.OrderBy(quadrant => quadrant.coordinates.DistanceTo(playerCoordinates)).ToArray();

            // set active only the nearest quadrants
            for (int i = 0; i < quadrants.Length; i++) {
                quadrants[i].SetActive(i < this.maxActiveQuadrants || this.player == null);
            }
        }

        private Coordinates2D[] NearestQuadrantCoordinates(int amount)
        {
            Coordinates2D[] coordinates = new Coordinates2D[amount];
            Coordinates2D playerCoordinates = this.playerCoordinates;

            // determine which quadrant the player is in
            int quadrantOffsetX = Mathf.CeilToInt((float)playerCoordinates.x / (float)this.width);
            int quadrantOffsetY = Mathf.CeilToInt((float)playerCoordinates.y / (float)this.length);

            // offset used to center the terrain so the middle column and row is the origin
            int originOffsetX = -_terrainLayout.columns / 2;
            int originOffsetY = -_terrainLayout.rows / 2;

            // clamp the quadrants to the max size
            int maxX = (int)(this.maxWidth / this.width) / 2;
            int maxY = (int)(this.maxLength / this.length) / 2;
            int offsetX = Mathf.Clamp(quadrantOffsetX + originOffsetX, -maxX, maxX - _terrainLayout.columns);
            int offsetY = Mathf.Clamp(quadrantOffsetY + originOffsetY, -maxY, maxY - _terrainLayout.rows);

            // gather quadrant coordinates
            for (int i = 0; i < amount; i++)
            {
                int column = _terrainLayout.ColumnAt(i) + offsetX;
                int row = _terrainLayout.RowAt(i) + offsetY;

                int x = column * (int)this.width;
                int y = row * (int)this.length;

                coordinates[i] = new Coordinates2D(x, y);
            }

            // order the coordinates by the distance to the player
            return coordinates.OrderBy(n => n.DistanceTo(playerCoordinates)).ToArray();
        }

        public void WorldCoordinatesFrom(Coordinates2D localCoordinates, out Coordinates3D worldCoordinates)
        {
            worldCoordinates = WorldDataAt(localCoordinates).worldCoordinates;
        }

        public void LocalCoordinatesFrom(Coordinates3D worldCoordinates, out Coordinates2D localCoordinates)
        {
            localCoordinates = new Coordinates2D(worldCoordinates.x, worldCoordinates.z);
        }

        public CubeTerrainBlock BlockAt(Coordinates2D localCoordinates)
        {
            return WorldDataAt(localCoordinates).block;
        }

        public CubeTerrainBlock BlockAt(Coordinates3D worldCoordinates)
        {
            return WorldDataAt(worldCoordinates).block;
        }

        internal CubeTerrainCoordinateGridData WorldDataAt(Coordinates2D localCoordinates)
        {
            return this.world[localCoordinates];
        }

        internal CubeTerrainCoordinateGridData WorldDataAt(Coordinates3D worldCoordinates)
        {
            Coordinates2D localCoordinates;
            LocalCoordinatesFrom(worldCoordinates, out localCoordinates);
            return this.world[localCoordinates];
        }

    }

}
