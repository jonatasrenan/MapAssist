using MapAssist.Helpers;
using MapAssist.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAssist.Types
{
    public class Path : IUpdatable<Path>
    {
        IntPtr _pPath = IntPtr.Zero;
        private Structs.Path _path;

        public Path(IntPtr pPath)
        {
            _pPath = pPath;
            Update();
        }

        public Path Update()
        {
            using (var processContext = GameManager.GetProcessContext())
            {
                _path = processContext.Read<Structs.Path>(_pPath);
            }
            return this;
        }

        public ushort DynamicX { get => _path.DynamicX; }
        public ushort DynamicY { get => _path.DynamicY; }
        public ushort StaticX { get => _path.StaticX; }
        public ushort StaticY { get => _path.StaticY; }
        public Room Room { get => new Room(_path.pRoom); }
}
}
