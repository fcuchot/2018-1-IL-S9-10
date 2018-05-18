using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Optim
{
    public abstract class SolutionInstance
    {
        readonly SolutionSpace _space;
        readonly IReadOnlyList<int> _coordinates;
        double _cost;

        protected SolutionInstance( SolutionSpace space, IReadOnlyList<int> coordinates )
        {
            _space = space;
            _coordinates = coordinates;
            _cost = -1.0;
        }

        public SolutionSpace Space => _space;


        IReadOnlyList<int> CreateNeighborCoordinates( int idxChange, bool increment )
        {
            var c = _coordinates.ToArray();
            c[idxChange] = increment ? c[idxChange] + 1 : c[idxChange] - 1;
            return c;
        }

        public IEnumerable<SolutionInstance> Neighbors
        {
            get
            {
                for( int i = 0; i < _space.Dimensions.Count; ++i )
                {
                    if( _coordinates[i] > 0 )
                        yield return _space.CreateInstance( CreateNeighborCoordinates( i, false ) );
                    if( _coordinates[i] < _space.Dimensions[i] - 1 )
                        yield return _space.CreateInstance( CreateNeighborCoordinates( i, true ) );
                }
            }
        }


        /// <summary>
        /// Gets the minimal cost among <see cref="Neighbors"/>.
        /// Never null since we always have at least one neighbor.
        /// </summary>
        /// <returns>The best neighbor.</returns>
        SolutionInstance GetBestAmongNeighbors() => Neighbors.Aggregate( ( b, c ) => b != null || c.Cost > b.Cost ? b : c );

        /// <summary>
        /// Gets the best solution instance reachable by following the Monte Carlo principle.
        /// It may be this solution.
        /// </summary>
        /// <returns>The best reachable solution.</returns>
        SolutionInstance GetBestMonteCarlo()
        {
            SolutionInstance candidate;
            var best = this;
            while( (candidate = GetBestAmongNeighbors()).Cost < best.Cost ) best = candidate;
            return best;
        }

        public IReadOnlyList<int> Coordinates => _coordinates;

        public double Cost
        {
            get
            {
                if( _cost < 0 ) _cost = ComputeCost();
                return _cost;
            }

        }

        protected abstract double ComputeCost();
    }
}
