using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine.Entities.GameComponents
{
    public class LayerComparer : Comparer<MovingEntity>
    {
        public override int Compare(MovingEntity x, MovingEntity y)
        {
            if (object.Equals(x, y))
                return 0; 
            return x.Layer.CompareTo(y.Layer);
        }
    }
}
