using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAssist.Interfaces
{
    public interface IUpdatable<T>
    {
        T Update();
    }
}
