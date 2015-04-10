using System;

namespace RemoveSeagulls
{
    public sealed class Settings
    {
        private Settings()
        {
            #if DEBUG

            IsDebugBuild   = true;
            IsPTRBuild     = false;
            IsReleaseBuild = false;

            Flair         += "ARIS LOCAL";

            #elif PTR

            IsDebugBuild   = false;
            IsPTRBuild     = true;
            IsReleaseBuild = false;

            Flair         += "ARIS PTR";

            #else

            IsDebugBuild   = false;
            IsPTRBuild     = false;
            IsReleaseBuild = true;

            Flair         += "ARIS";

            #endif

            Tag = String.Format("[{0}] Remove Seagulls", Flair);
        }

        private static readonly Settings _Instance = new Settings();
        public static Settings Instance { get { return _Instance; } }

        public readonly string Flair;
        public readonly string Tag;

        public readonly bool IsDebugBuild;
        public readonly bool IsPTRBuild;
        public readonly bool IsReleaseBuild;
    }
}