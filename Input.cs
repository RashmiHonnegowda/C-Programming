using System.Collections;
using System.Windows.Forms;

namespace Snake
{
    internal class Input
    {
        private static Hashtable KeyTable = new Hashtable();

        public static bool KeyPressed(Keys Key)
        {
            if(KeyTable[Key]==null)
            {
                return false;
            }
            return (bool)KeyTable[Key];
        }

        public static void ChangeState(Keys key, bool state)
        {
            KeyTable[key] = state;
        }
    }
}
