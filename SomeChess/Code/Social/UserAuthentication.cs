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
                PlayerStorage storage = PlayerStorage.GetInstence();
                storage.UpdateUserList(player);
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
                    CurrentPlayer = player;
                    return player;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fetch user went wrong");
                Console.WriteLine(ex.ToString());

                return null;
            }
        }



        public async Task<Player?> CreateNewPlayer(string name)
        {
            PlayerStorage storage = PlayerStorage.GetInstence();

            bool isFree = storage.IsNameFree(name);


            if (isFree)
            {
                Player newPlayer = storage.CreatePlayer(name);
                newPlayer.PlayersMatch = 0;
                if(CurrentPlayer != null)
                    storage.RemovePlayerByID(CurrentPlayer.ID);

                CurrentPlayer = newPlayer;
                return newPlayer;
            }
            else
            {
                return null;
            }
        }


        public async Task ClearBrowserUserDataAsync()
        {
            await _protectedPlayerStorage.DeleteAsync(_storageKey);
        }




    }
}
