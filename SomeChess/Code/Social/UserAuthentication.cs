using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;

namespace SomeChess.Code.Social
{
    public class UserAuthentication
    {
        public readonly ProtectedLocalStorage _protectedPlayerStorage;
        public readonly string _storageKey = "chess-googolplex";

        public Player? CurrentPlayer { get; set; }


        public UserAuthentication(ProtectedLocalStorage storage)
        {
            _protectedPlayerStorage = storage;
        }


        public async Task PersistPlayerAsync(Player player)
        {
            try
            {
                await ClearBrowserUserDataAsync();
                string userJson = JsonConvert.SerializeObject(player);
                await _protectedPlayerStorage.SetAsync(_storageKey, userJson);

                Console.WriteLine("PersistPlayerAsync(): user wassuccessfully persisted");
            }
            catch
            {
                Console.WriteLine("PersistPlayerAsync(): doesnt work");
            }
            
            //await UpdateCurrentPlayerAsync();
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
                Console.WriteLine("CurrentUser is NULL");
                CurrentPlayer = null;
            }
        }



        public async Task<Player?> FetchPlayerAsync()
        {
            try
            {
                var fetchedUserResult = await _protectedPlayerStorage.GetAsync<string>(_storageKey);

                if (fetchedUserResult.Success)
                {
                    var player = JsonConvert.DeserializeObject<Player>(fetchedUserResult.Value);
                    Console.WriteLine("FetchPlayerAsync(): fetching of user was successfull");
                    CurrentPlayer = player;
                    return player;
                }

                Console.WriteLine("FetchPlayerAsync(): fetched user in NULL");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FetchPlayerAsync(): fetching of user went wrong");
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
