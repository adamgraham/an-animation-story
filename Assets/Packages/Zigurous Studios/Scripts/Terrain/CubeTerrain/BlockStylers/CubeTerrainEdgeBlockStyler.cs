using UnityEngine;
using System.Linq;

namespace Zigurous.Terrain.CubeTerrain
{
    public sealed class CubeTerrainEdgeBlockStyler : CubeTerrainBlockStyler
    {
        public Renderer materialRenderer;
        public Material[] materials;

        private void Awake()
        {
            if (this.materialRenderer == null) {
                this.materialRenderer = GetComponentInChildren<Renderer>();
            }
        }

        internal override void Style(CubeTerrainBlock block, CubeTerrain terrain)
        {
            if (this.materials == null || this.materialRenderer == null) { return; }
            int index = terrain.WorldDataAt(block.coordinates).edgeDistance;
            index = Mathf.Clamp(index, 0, this.materials.Length - 1);
            this.materialRenderer.material = this.materials[index];
        }

    }

}
