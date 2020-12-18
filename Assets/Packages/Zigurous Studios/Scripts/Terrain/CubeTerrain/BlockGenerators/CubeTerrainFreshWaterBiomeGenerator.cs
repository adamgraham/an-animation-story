using UnityEngine;

namespace Zigurous.Terrain.CubeTerrain
{
    public class CubeTerrainFreshWaterBiomeGenerator : CubeTerrainBlockGenerator
    {
        public CubeTerrainGrassBlock grassBlock;
        private ObjectPool<CubeTerrainBlock> _grassObjectPool;

        public CubeTerrainWaterBlock waterBlock;
        private ObjectPool<CubeTerrainBlock> _waterObjectPool;

        protected virtual void Awake()
        {
            _grassObjectPool = new ObjectPool<CubeTerrainBlock>(GenerateGrassBlock);
            _waterObjectPool = new ObjectPool<CubeTerrainBlock>(GenerateWaterBlock);
        }

        protected virtual void OnDestroy()
        {
            this.grassBlock = null;
            this.waterBlock = null;

            _grassObjectPool = null;
            _waterObjectPool = null;
        }

        protected override ObjectPool<CubeTerrainBlock> ObjectPoolFor(CubeTerrainBlock block)
        {
            if (block is CubeTerrainWaterBlock) {
                return _waterObjectPool;
            } else {
                return _grassObjectPool;
            }
        }

        protected override ObjectPool<CubeTerrainBlock> ObjectPoolFor(Coordinates3D coordinates, CubeTerrain terrain)
        {
            if (coordinates.y < terrain.seaLevel) {
                return _waterObjectPool;
            } else {
                return _grassObjectPool;
            }
        }

        private CubeTerrainBlock GenerateGrassBlock()
        {
            return Instantiate(this.grassBlock);
            
        }

        private CubeTerrainBlock GenerateWaterBlock()
        {
            return Instantiate(this.waterBlock);
        }

    }

}
