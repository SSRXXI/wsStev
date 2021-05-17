using System.Collections.Generic;

namespace Cl.Agp.Stev.Utils.Dictionaries
{
    public static class SpecialCharactersDictionary
    {
        public static Dictionary<string, string> SpecialCharacterList()
        {
            Dictionary<string, string> characters = new Dictionary<string, string>();
            characters.Add("&", "&amp;");
            characters.Add("<", "&lt;");
            characters.Add(">", "&gt;");
            return characters;
        }
    }
}
