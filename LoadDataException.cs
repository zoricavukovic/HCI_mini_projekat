using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjekatHCI
{
    class LoadDataException:Exception
    {
        public LoadDataException()
        : base(String.Format("Loading failed! Please try again."))
        {

        }
    }
}
