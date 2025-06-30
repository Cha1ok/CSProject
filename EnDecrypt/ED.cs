using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace EnDecrypt
{
    public static class ED
    {
        private const int Shift = 3;

        public static string Encrypt(string message)
        {
            char[] chars = message.ToCharArray();
            for(int i=0;i<chars.Length;i++)
            {
                char c = chars[i];
                if (char.IsLetter(c))
                {
                    char basechar = char.IsUpper(c) ? 'A' : 'a';
                    chars[i]=(char)(((c-basechar+Shift)%26)+basechar);
                }
            }
            return new string(chars);
        }

        public static string Decrypt(string message)
        {
            char[] chars = message.ToCharArray();
            for(int i=0;i< chars.Length;i++)
            {
                char c = chars[i];
                if (char.IsLetter(c))
                {
                    char basechar=char.IsUpper(c) ? 'A' : 'a';
                    chars[i]=(char)(((c-basechar-Shift+26)%26)+basechar) ;
                }
            }
            return new string((chars));
        }
    }
}
