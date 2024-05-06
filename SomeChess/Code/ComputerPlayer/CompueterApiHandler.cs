namespace SomeChess.Code.ComputerPlayer
{
    public class ComputerApiHandler
    {
        public string ComputersMove;
        public string Key;
        public static string Port;

        private static ComputerApiHandler _instance;
        private static HttpClient client = new HttpClient();

        public static ComputerApiHandler GetInstance()
        {
            if (_instance == null)
                _instance = new ComputerApiHandler();

            return _instance;
        }

        private ComputerApiHandler()
        {
            StringContent content = new StringContent("INITIALIZE~");
            Port = "5555";
            using HttpResponseMessage response1 = client.PostAsync($"http://localhost:{Port}/", content).Result;
            response1.EnsureSuccessStatusCode();
            string responseBody = response1.Content.ReadAsStringAsync().Result;
            Port = responseBody;
        }

        public static string CreateInstance()
        {
            StringContent content = new StringContent("CREATEINSTANCE~");

            using HttpResponseMessage response2 = client.PostAsync($"http://localhost:{Port}/", content).Result;
            response2.EnsureSuccessStatusCode();
            string responseBody = response2.Content.ReadAsStringAsync().Result;
            string Key = responseBody;
            return Key;
        }

        public static string MakeTurn(string key, string fromId, string toId)
        {
            ComputerApiHandler instance = GetInstance();
            StringContent content = new StringContent($"MAKEMOVE~{key}~{fromId}{toId}");

            using HttpResponseMessage myMove = client.PostAsync($"http://localhost:{Port}/", content).Result;
            myMove.EnsureSuccessStatusCode();
            string MyMove = myMove.Content.ReadAsStringAsync().Result;

            content = new StringContent($"GETENGINEMOVE~{key}~");

            using HttpResponseMessage engineMove = client.PostAsync($"http://localhost:{Port}/", content).Result;
            engineMove.EnsureSuccessStatusCode();
            string responseBody = engineMove.Content.ReadAsStringAsync().Result;

            string ComputersMove = responseBody;
            return ComputersMove;
        }
    }
}
