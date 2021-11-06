using MapAssist.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAssist.Types
{
    public abstract class ProcessObject
    {
        protected IntPtr _processHandle;
        public ProcessObject(IntPtr processHandle)
        {
            _processHandle = processHandle;
        }
    }
}
