using MapAssist.Helpers;
using MapAssist.Interfaces;
using System;

namespace MapAssist.Types
{
    public class UiSettings : IUpdatable<UiSettings>
    {
        IntPtr _pUiSettings = IntPtr.Zero;
        private Structs.UiSettings _uiSettings;

        public UiSettings(IntPtr pUiSettings)
        {
            _pUiSettings = pUiSettings;
            Update();
        }

        public UiSettings Update()
        {
            using (var processContext = GameManager.GetProcessContext())
            {
                _uiSettings = processContext.Read<Structs.UiSettings>(_pUiSettings);
            }
            return this;
        }

        public bool MapShown { get => _uiSettings.MapShown; }
    }
}
