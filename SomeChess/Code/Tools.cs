using System.Text;

namespace SomeChess.Code
{
    public class Tools
    {
        public static string Create16DigitID()
        {
            Random rndm = new Random();
            var builder = new StringBuilder();
            while (builder.Length < 16)
            {
                builder.Append(rndm.Next(10).ToString());
            }

            return builder.ToString();
        }
    }
}
