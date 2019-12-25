using System.Collections.Generic;
using System.Collections.ObjectModel;
using Optimization.Core;

namespace Optimization.DailyModel
{
    public class Goods: ReadOnlyCollection<IGood>, IGoods
    {
        public Goods(IList<IGood> list) : base(list)
        {
        }
    }
}