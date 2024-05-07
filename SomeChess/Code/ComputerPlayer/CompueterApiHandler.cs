namespace SomeChess.Code.ComputerPlayer
{
    public class ComputerApiHandler
    {

        public string ComputersMove;
        public static string Key;
        public static string Port = "5555";

        private static ComputerApiHandler _instance;
        private static HttpClient client = new HttpClient();

        public ComputerApiHandler GetInstance()
        {
            if (_instance == null)
                _instance = new ComputerApiHandler();

            return _instance;
        }

        public async Task<string> CreateInstance()
        {
            StringContent content = new StringContent("CREATEINSTANCE~");

            using HttpResponseMessage response2 = await client.PostAsync($"http://127.0.0.1:{Port}/", content);
            response2.EnsureSuccessStatusCode();
            string responseBody = response2.Content.ReadAsStringAsync().Result;
            string Key = responseBody;
            return Key;
        }

        public async Task<string> MakeTurn(string key, string fromId, string toId)
        {
            ComputerApiHandler instance = GetInstance();
            StringContent content = new StringContent($"MAKEMOVE~{key}~{fromId}{toId}");

            using HttpResponseMessage myMove = await client.PostAsync($"http://127.0.0.1:{Port}/", content);
            myMove.EnsureSuccessStatusCode();
            string MyMove = myMove.Content.ReadAsStringAsync().Result;

            content = new StringContent($"GETENGINEMOVE~{key}~");

            using HttpResponseMessage engineMove = await client.PostAsync($"http://127.0.0.1:{Port}/", content);
            engineMove.EnsureSuccessStatusCode();
            string responseBody = engineMove.Content.ReadAsStringAsync().Result;

            string ComputersMove = responseBody;
            return ComputersMove;
        }
    }
}
