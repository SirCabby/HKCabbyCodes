namespace CabbyCodes
{
    public class CodeState
    {
        private static CodeState instance;

        public static CodeState Get()
        {
            instance ??= new CodeState();
            return instance;
        }

        public void Register()
        {

        }
    }
}
