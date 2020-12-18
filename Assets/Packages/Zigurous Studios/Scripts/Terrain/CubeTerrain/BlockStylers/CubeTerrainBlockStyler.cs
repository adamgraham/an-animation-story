using UnityEngine;

namespace Zigurous.Terrain.CubeTerrain
{
    [RequireComponent(typeof(CubeTerrainBlock))]
    public abstract class CubeTerrainBlockStyler : MonoBehaviour
    {
        internal abstract void Style(CubeTerrainBlock block, CubeTerrain terrain);
    }

}
