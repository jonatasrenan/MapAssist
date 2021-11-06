using MapAssist.Helpers;
using MapAssist.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAssist.Types
{
    public class RoomEx : IUpdatable<RoomEx>
    {
        IntPtr _pRoomEx = IntPtr.Zero;
        private Structs.RoomEx _roomEx;

        public RoomEx(IntPtr pRoomEx)
        {
            _pRoomEx = pRoomEx;
            Update();
        }

        public RoomEx Update()
        {
            using (var processContext = GameManager.GetProcessContext())
            {
                _roomEx = processContext.Read<Structs.RoomEx>(_pRoomEx);
            }
            return this;
        }

        public Level Level { get => new Level(_roomEx.pLevel);  }
    }
}
