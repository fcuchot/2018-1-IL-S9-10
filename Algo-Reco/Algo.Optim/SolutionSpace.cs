using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Optim
{
    public abstract class SolutionSpace
    {
        readonly Random _random;

        public SolutionSpace( int randomSeed )
        {
            _random = new Random( randomSeed );
        }

        public IReadOnlyList<int> Dimensions { get; private set; }

        public double Cardinality => Dimensions.Aggregate( 1, ( acc, value ) => acc * value );

        public SolutionInstance GetRandomInstance()
        {

        }

        protected void Initialize( IReadOnlyList<int> spaceDimensions )
        {
            if( spaceDimensions == null ) throw new ArgumentNullException( nameof(spaceDimensions) );
            Dimensions = spaceDimensions;
        }
    }
}
