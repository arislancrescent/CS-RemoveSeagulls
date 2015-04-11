using System;

namespace RemoveSeagulls
{
    public sealed class Settings
    {
        private Settings()
        {
            Tag = "[ARIS] Remove Seagulls";
        }

        private static readonly Settings _Instance = new Settings();
        public static Settings Instance { get { return _Instance; } }

        public readonly string Tag;
    }
}