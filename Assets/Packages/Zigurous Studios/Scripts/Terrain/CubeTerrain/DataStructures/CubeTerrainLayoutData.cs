using System.Linq;

namespace Zigurous.Terrain.CubeTerrain
{
    internal struct CubeTerrainLayoutData
    {
        public int columns { get; private set; }
        public int rows { get; private set; }

        public CubeTerrainLayoutData(CubeTerrain terrain)
        {
            uint[] factors = terrain.amountQuadrants.GetFactors().OrderBy(n => n).ToArray();
            int factorsCount = factors.Count();

            switch (factorsCount)
            {
                case 0: // amount of quadrants = 0
                    this.columns = this.rows = 0;
                    break;

                case 1: // amount of quadrants = 1
                    this.columns = this.rows = 1;
                    break;

                case 2: // amount of quadrants is a prime number
                    this.columns = (int)factors.Last();
                    this.rows = (int)factors.First();
                    break;

                default:
                    {
                        if (((uint)factorsCount).IsOdd())
                        {
                            // amount of quadrants is a square number
                            uint square = factors.Skip(factorsCount / 2).First();
                            this.columns = this.rows = (int)square;
                        }
                        else
                        {
                            // amount of quadrants is a composite number
                            factors = factors.Skip(factorsCount / 2 - 1).Take(2).ToArray();
                            this.columns = (int)factors.Last();
                            this.rows = (int)factors.First();
                        }
                        break;
                    }
            }
        }

        public int ColumnAt(int index)
        {
            return index / this.rows;
        }

        public int RowAt(int index)
        {
            return index % this.rows;
        }

    }

}
