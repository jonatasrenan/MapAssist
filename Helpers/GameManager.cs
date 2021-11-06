using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapAssist.Structs;
using MapAssist.Types;

namespace MapAssist.Helpers
{
    public static class GameManager
    {
        private static readonly string ProcessName = Encoding.UTF8.GetString(new byte[] { 68, 50, 82 });
        private static IntPtr PlayerUnitPtr = IntPtr.Zero;
        private static Types.UnitAny _PlayerUnit = default;
        private static int LastProcessId = 0;
        private static IntPtr _MainWindowHandle = IntPtr.Zero;
        private static ProcessContext _ProcessContext;
        private static Process GameProcess;

        public static ProcessContext GetProcessContext()
        {
            if (_ProcessContext != null && _ProcessContext.OpenContext > 0)
            {
                _ProcessContext.OpenContext++;
                return _ProcessContext;
            }

            // Console.WriteLine("Making new context.");

            if (GameProcess == null)
            {
                Process[] processes = Process.GetProcessesByName(ProcessName);

                Process gameProcess = null;

                IntPtr windowInFocus = WindowsExternal.GetForegroundWindow();
                if (windowInFocus == IntPtr.Zero)
                {
                    gameProcess = processes.FirstOrDefault();
                }
                else
                {
                    gameProcess = processes.FirstOrDefault(p => p.MainWindowHandle == windowInFocus);
                }

                if (gameProcess == null)
                {
                    gameProcess = processes.FirstOrDefault();
                }

                if (gameProcess == null)
                {
                    throw new Exception("Game process not found.");
                }

                // If changing processes we need to re-find the player
                if (gameProcess.Id != LastProcessId)
                {
                    ResetPlayerUnit();
                }

                GameProcess = gameProcess;
            }

            LastProcessId = GameProcess.Id;
            _MainWindowHandle = GameProcess.MainWindowHandle;
            _ProcessContext = new ProcessContext(GameProcess);
            return _ProcessContext;
        }

        public static IntPtr MainWindowHandle { get => _MainWindowHandle; }

        public static Types.UnitAny PlayerUnit
        {
            get
            {
                using (var processContext = GetProcessContext())
                {
                    if (Equals(_PlayerUnit, default(Types.UnitAny)) || PlayerUnitPtr != IntPtr.Zero)
                    {
                        IntPtr aPlayerUnit = processContext.Read<IntPtr>(PlayerUnitPtr);
                        if (aPlayerUnit != IntPtr.Zero)
                        {
                            return _PlayerUnit.Update();
                        }
                        else
                        {
                            ResetPlayerUnit();
                        }
                    }


                    if (Equals(_PlayerUnit, default(Types.UnitAny)))
                    {
                        foreach (var pUnitAny in UnitHashTable.UnitTable)
                        {
                            var unitAny = new Types.UnitAny(pUnitAny);

                            while (unitAny.IsValid())
                            {
                                if (unitAny.IsPlayerUnit())
                                {
                                    PlayerUnitPtr = pUnitAny;
                                    _PlayerUnit = unitAny;
                                    break;
                                }
                                unitAny = unitAny.ListNext;
                            }

                            if (PlayerUnitPtr != IntPtr.Zero)
                            {
                                return _PlayerUnit = unitAny;
                            }
                        }
                    }

                    ResetPlayerUnit();
                    throw new Exception("Player unit not found.");
                }
            }
        }

        public static UnitHashTable UnitHashTable
        {
            get
            {
                using (var processContext = GetProcessContext())
                {
                    return processContext.Read<UnitHashTable>(processContext.FromOffset(Offsets.UnitHashTable));
                }
            }
        }

        public static Types.UiSettings UiSettings
        {
            get
            {
                using (var processContext = GetProcessContext())
                {
                    return new Types.UiSettings(processContext.FromOffset(Offsets.UiSettings));
                }
            }
        }

        public static void ResetPlayerUnit()
        {
            _PlayerUnit = default;
            PlayerUnitPtr = IntPtr.Zero;
        }
    }
}
