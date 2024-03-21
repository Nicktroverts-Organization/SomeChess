using System.Text;
using System.Xml.Linq;
using SomeChess.Code;

namespace SomeChess.Code.Social
{
    public class Player
    {
        public string Name { get; set; }

        public string ID { get; set; }

        public Player(string name, string id)
        {
            Name = name;
            ID = id;
        }


    }

    public sealed class PlayerStorage
    {
        private List<Player> _players = new();

        private static PlayerStorage instance;

        private PlayerStorage() { }

        public static PlayerStorage GetInstence()
        {
            if (instance == null)
            {
                instance = new PlayerStorage();
            }
            return instance;
        }

        public Player GetPlayerByName(string name)
        {
            try
            {
                Player player = _players.Where(p => p.Name == name).FirstOrDefault();
                return player;
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot find a player by ID: " + e);
                return null;
            }
        }




        public Player CreatePlayer(string name, bool addToStorage = true)
        {
            string newID = String.Empty;
            bool isIdUnique = false;

            while(!isIdUnique)
            {
                newID = Tools.Create16DigitID();

                if (GetPlayerByName(newID) == null)
                    isIdUnique = true;
            }

            Player newPlayer = new(name, newID);

            if (addToStorage)
                _players.Add(newPlayer);
            
            return newPlayer;
        }


        public void RemovePlayerByID(string id)
        {
            try
            {
                Player player = _players.Where(p => p.ID == id).FirstOrDefault();
                _players.Remove(player);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot remove a player by ID: " + e);
            }
        }
    }

}
