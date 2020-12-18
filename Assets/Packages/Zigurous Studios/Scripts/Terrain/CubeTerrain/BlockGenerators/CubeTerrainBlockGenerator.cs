using UnityEngine;

namespace Zigurous.Terrain.CubeTerrain
{
    public abstract class CubeTerrainBlockGenerator : MonoBehaviour
    {
        protected abstract ObjectPool<CubeTerrainBlock> ObjectPoolFor(CubeTerrainBlock block);
        protected abstract ObjectPool<CubeTerrainBlock> ObjectPoolFor(Coordinates3D coordinates, CubeTerrain terrain);

        internal virtual void Store(CubeTerrainBlock block, CubeTerrain terrain, CubeTerrainQuadrant quadrant)
        {
            ObjectPool<CubeTerrainBlock> objectPool = ObjectPoolFor(block);
            objectPool.Enqueue(block);
            block.DeconfigureFrom(terrain, quadrant);
            terrain.WorldDataAt(block.coordinates).block = null;
        }

        internal virtual CubeTerrainBlock GenerateAt(Coordinates3D coordinates, CubeTerrain terrain, CubeTerrainQuadrant quadrant)
        {
            ObjectPool<CubeTerrainBlock> objectPool = ObjectPoolFor(coordinates, terrain);
            CubeTerrainBlock block = objectPool.Dequeue();
            block.ConfigureOn(terrain, quadrant, coordinates);
            terrain.WorldDataAt(coordinates).block = block;
            return block;
        }

    }

}
