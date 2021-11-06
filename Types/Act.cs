using MapAssist.Helpers;
using MapAssist.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAssist.Types
{
    public class Act : IUpdatable<Act>
    {
        IntPtr _pAct = IntPtr.Zero;
        private Structs.Act _act;

        public Act(IntPtr pAct)
        {
            _pAct = pAct;
            Update();
        }

        public Act Update()
        {
            using (var processContext = GameManager.GetProcessContext())
            {
                _act = processContext.Read<Structs.Act>(_pAct);
            }
            return this;
        }

        public uint MapSeed { get => _act.MapSeed; }
        public uint ActId { get => _act.ActId; }
        public ActMisc ActMisc { get => new ActMisc(_act.pActMisc); }
    }
}
