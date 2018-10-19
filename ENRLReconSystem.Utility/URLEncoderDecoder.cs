using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.Utility
{
    public class URLEncoderDecoder
    {
        public static string Encode(string encode)
        {
            
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encode);
            return Convert.ToBase64String(encoded);
        }

        public static string Decode(string decode)
        {
            byte[] encoded = Convert.FromBase64String(decode);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }
    }
}
