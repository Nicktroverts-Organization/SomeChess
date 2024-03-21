using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;

namespace SomeChess.Code.Social
{
    public class UserAuthentication
    {
        public readonly ProtectedLocalStorage _protectedPlayerStorage;
        public readonly string _storageKey = "chess-googolplex";

        public Player? CurrentPlayer { get; private set; }


        public UserAuthentication(ProtectedLocalStorage storage)
        {
            _protectedPlayerStorage = storage;
        }


        public async Task PersistPlayerAsync(Player player)
        {
            await ClearBrowserUserDataAsync();
            string userJson = JsonConvert.SerializeObject(player);
            await _protectedPlayerStorage.SetAsync(_storageKey, userJson);

            await UpdateCurrentPlayerAsync();
        }


        public async Task UpdateCurrentPlayerAsync()
        {
            Player? player = await FetchPlayerAsync();

            if (player != null)
            {
                CurrentPlayer = player;
            }
            else
            {
                Console.WriteLine("CurrentUser is null");
                CurrentPlayer = null;
            }
        }



        public async Task<Player?> FetchPlayerAsync()
        {
            try
            {
                var fetchedUserResult = await _protectedPlayerStorage.GetAsync<string>(_storageKey);

                if (fetchedUserResult.Success && !string.IsNullOrEmpty(fetchedUserResult.Value))
                {
                    var player = JsonConvert.DeserializeObject<Player>(fetchedUserResult.Value);
                    Console.WriteLine("Fetch user was successfull");
                    return player;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fetch user went wrong");
                Console.WriteLine(ex.ToString());
            }

            return null;
        }



        public async Task<Player> AuthenticatePlayer(string name)
        {
            PlayerStorage storage = PlayerStorage.GetInstence();

            Player player = storage.GetPlayerByName(name);


            if (player == null)
            {
                Player newPlayer = storage.CreatePlayer(name);
                return newPlayer;
            }
            else
            {
                return player;
            }
        }


        public async Task ClearBrowserUserDataAsync()
        {
            await _protectedPlayerStorage.DeleteAsync(_storageKey);
        }




    }
}
