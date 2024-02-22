using System.Text;
using System.Xml.Linq;

namespace SomeChess.Code.Social
{
    public class Player
    {
        public string Name { get; set; }

        public string ID { get; set; }

        public Player(string name, string id)
        {
            Name = name;

        }


    }

    public class PlayerStorage
    {
        private List<Player> _players;

        public Player GetPlayerByID(string id)
        {
            try
            {
                Player player = _players.Where(p => p.ID == id).FirstOrDefault();
                return player;
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot find a player by ID: " + e);
                return null;
            }
        }

        private string Create16DigitString()
        {
            Random rndm = new Random();
            var builder = new StringBuilder();
            while (builder.Length < 16)
            {
                builder.Append(rndm.Next(10).ToString());
            }
            
            return builder.ToString();
        }


        public Player CreatePlayer(string name, bool addToStorage = true)
        {
            string newID = String.Empty;
            bool isIdUnique = false;

            while(!isIdUnique)
            {
                newID = Create16DigitString();

                if (GetPlayerByID(newID) == null)
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
