using ICities;

namespace RemoveSeagulls
{
    public class Identity : IUserMod
    {
        public string Name
        {
            get { return Settings.Instance.Tag; }
        }

        public string Description
        {
            get { return "Nonpermanently removes all seagulls. Disable to get the seagulls back."; }
        }
    }
}