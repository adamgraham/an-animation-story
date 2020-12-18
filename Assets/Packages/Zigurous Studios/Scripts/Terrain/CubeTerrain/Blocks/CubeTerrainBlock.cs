using UnityEngine;

namespace Zigurous.Terrain.CubeTerrain
{
    public abstract class CubeTerrainBlock : MonoBehaviour
    {
        internal new Transform transform;

        public CubeTerrainBlockStyler[] styling;

        public Coordinates3D coordinates { get; internal set; }

        protected virtual void Awake()
        {
            this.transform = GetComponent<Transform>();
        }

        internal void PositionOn(CubeTerrainQuadrant quadrant, Coordinates3D coordinates)
        {
            this.coordinates = coordinates;
            this.transform.parent = quadrant.gameObject.transform;
            this.transform.position = coordinates.ToVector3();
        }

        internal virtual void ConfigureOn(CubeTerrain terrain, CubeTerrainQuadrant quadrant, Coordinates3D coordinates)
        {
            PositionOn(quadrant, coordinates);
            this.gameObject.SetActive(true);
        }

        internal virtual void DeconfigureFrom(CubeTerrain terrain, CubeTerrainQuadrant quadrant)
        {
            this.gameObject.SetActive(false);
        }

        internal virtual void StyleOn(CubeTerrain terrain)
        {
            if (this.styling == null) { return; }

            for (int i = 0; i < this.styling.Length; i++) {
                this.styling[i].Style(this, terrain);
            }
        }

    }

}
