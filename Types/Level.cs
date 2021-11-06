using MapAssist.Helpers;
using MapAssist.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAssist.Types
{
    public class Level : IUpdatable<Level>
    {
        IntPtr _pLevel = IntPtr.Zero;
        private Structs.Level _level;

        public Level(IntPtr pLevel)
        {
            _pLevel = pLevel;
            Update();
        }

        public Level Update()
        {
            using (var processContext = GameManager.GetProcessContext())
            {
                _level = processContext.Read<Structs.Level>(_pLevel);
            }
            return this;
        }

        public Area LevelId { get => _level.LevelId; }

    }
}
