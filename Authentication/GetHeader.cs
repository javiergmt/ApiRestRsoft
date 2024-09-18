using System.Drawing;

namespace ApiRestRs.Authentication
{
    public class GetHeader
    {
        public static string? AnalizarHeaders(IHeaderDictionary headers)
        {
            headers.TryGetValue("bd", out var bd);
            {
                //Console.WriteLine(bd);
                if (bd == "")
                {
                    return null;
                }
                else
                {
                    return bd;
                }
              
            }

        }
    }
}
