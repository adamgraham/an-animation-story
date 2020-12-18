using UnityEngine;
using System;
using System.Collections.Generic;

namespace Zigurous.Terrain.CubeTerrain
{
    internal class CubeTerrainQuadrant
    {
        public GameObject gameObject { get; private set; }

        private Coordinates2D? _activeCoordinates;
        private Coordinates2D? _repositionCoordinates;
        public Coordinates2D coordinates
        {
            get
            {
                if (_repositionCoordinates != null) {
                    return _repositionCoordinates.Value;
                } else if (_activeCoordinates != null) {
                    return _activeCoordinates.Value;
                } else {
                    return Coordinates2D.zero;
                }
            }
        }
        
        private List<CubeTerrainBlock> _generatedBlocks;
        private WeakReference _terrainWeakReference;
        private CubeTerrain cubeTerrain
        {
            get
            {
                if (_terrainWeakReference != null && _terrainWeakReference.IsAlive) {
                    return (CubeTerrain)_terrainWeakReference.Target;
                } else {
                    return null;
                }
            }
        }

        private CubeTerrainQuadrant() { }

        public CubeTerrainQuadrant(CubeTerrain terrain)
        {
            this.gameObject = new GameObject();
            this.gameObject.name = "Quadrant";
            this.gameObject.transform.parent = terrain.transform;

            _generatedBlocks = new List<CubeTerrainBlock>();
            _terrainWeakReference = new WeakReference(terrain);
        }

        public void SetActive(bool active)
        {
            if (this.gameObject.activeSelf != active) {
                this.gameObject.SetActive(active);
            }
            
            RepositionIfNeeded();
        }

        public void SetNeedsRepositionAt(Coordinates2D coordinates)
        {
            _repositionCoordinates = coordinates;
        }

        public void RepositionIfNeeded()
        {
            if (this.gameObject.activeSelf && _repositionCoordinates != null)
            {
                if (!_activeCoordinates.Equals(_repositionCoordinates)) {
                    PositionAt(_repositionCoordinates.Value);
                }
                
                _repositionCoordinates = null;
            }
        }

        private void PositionAt(Coordinates2D coordinates)
        {
            _activeCoordinates = coordinates;

            #if UNITY_EDITOR
            this.gameObject.name = "Quadrant: " + _activeCoordinates.ToString();
            #endif

            CubeTerrain terrain = this.cubeTerrain;
            if (terrain == null || terrain.blockGenerator == null) { return; }

            if (terrain.isRegenerative) {
                DegenerateBlocksOn(terrain);
            }

            if (_generatedBlocks.Count <= 0) {
                GenerateBlocksOn(terrain);
            } else {
                PositionBlocksOn(terrain);
            }

            StyleBlocksOn(terrain);
        }

        private void DegenerateBlocksOn(CubeTerrain terrain)
        {
            _generatedBlocks.ForEach(block => {
                terrain.blockGenerator.Store(block, terrain, quadrant: this);
            });

            _generatedBlocks.Clear();
        }

        private void GenerateBlocksOn(CubeTerrain terrain)
        {
            Coordinates3D worldCoordinates = Coordinates3D.zero;
            Coordinates2D localCoordinates = _activeCoordinates.Value;

            // calculate coordinate bounds
            int startX = localCoordinates.x;
            int startY = localCoordinates.y;
            int endX = localCoordinates.x + (int)terrain.width;
            int endY = localCoordinates.y + (int)terrain.length;

            // position all blocks
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    // get world coordinates
                    localCoordinates.x = x;
                    localCoordinates.y = y;
                    terrain.WorldCoordinatesFrom(localCoordinates, out worldCoordinates);

                    // generate block at coordinates
                    CubeTerrainBlock block = terrain.blockGenerator.GenerateAt(worldCoordinates, terrain, quadrant: this);
                    _generatedBlocks.Add(block);
                }
            }
        }

        private void PositionBlocksOn(CubeTerrain terrain)
        {
            Coordinates3D worldCoordinates = Coordinates3D.zero;
            Coordinates2D localCoordinates = _activeCoordinates.Value;

            // calculate coordinate bounds
            int startX = localCoordinates.x;
            int startY = localCoordinates.y;
            int endX = localCoordinates.x + (int)terrain.width;
            int endY = localCoordinates.y + (int)terrain.length;
            int x = startX;
            int y = startY;

            for (int i = 0; i < _generatedBlocks.Count; i++)
            {
                // get world coordinates
                localCoordinates.x = x;
                localCoordinates.y = y;
                terrain.WorldCoordinatesFrom(localCoordinates, out worldCoordinates);

                // position block
                _generatedBlocks[i].PositionOn(this, worldCoordinates);

                // update indexes
                if (++y == endY)
                {
                    y = startY; // reset to first row
                    x++; // move to next column
                }
            }
        }

        private void StyleBlocksOn(CubeTerrain terrain)
        {
            _generatedBlocks.ForEach(block => {
                block.StyleOn(terrain);
            });
        }

    }

}
