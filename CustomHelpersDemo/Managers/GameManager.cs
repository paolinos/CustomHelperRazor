using CustomHelpersDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomHelpersDemo.Managers
{
    public class GameManager
    {
        private static List<Game> list = new List<Game>();

        public static void fill()
        {
            //list.Clear();
            Add( new Game(){
                Name = "Day of the Tentacle",
                Publisher = "LucasArts",
                ReleaseDate = new DateTime(1993,06,25)
            });
            Add(new Game()
            {
                Name = "Maniac Mansion",
                Publisher = "Lucasfilm Games",
                ReleaseDate = new DateTime(1993, 10, 01)
            });
            Add(new Game()
            {
                Name = "Sam & Max Hit the Road",
                Publisher = "LucasArts",
                ReleaseDate = new DateTime(1993, 01, 01)
            });
            Add(new Game()
            {
                Name = "Full Throttle",
                Publisher = "LucasArts",
                ReleaseDate = new DateTime(1995, 04, 30)
            });
            Add(new Game()
            {
                Name = "Max Payne",
                Publisher = "Rockstar Games",
                ReleaseDate = new DateTime(2001, 01, 01)
            });
            Add(new Game()
            {
                Name = "Max Payne 2: The Fall of Max Payne",
                Publisher = "Rockstar Games",
                ReleaseDate = new DateTime(2003, 01, 01)
            });
            Add(new Game()
            {
                Name = "Max Payne 3",
                Publisher = "Rockstar Games",
                ReleaseDate = new DateTime(2012, 05, 15)
            });
            Add(new Game()
            {
                Name = "Grand Theft Auto",
                Publisher = "Rockstar Games",
                ReleaseDate = new DateTime(1997, 01, 01)
            });
            Add(new Game()
            {
                Name = "Grand Theft Auto 2",
                Publisher = "Rockstar Games",
                ReleaseDate = new DateTime(1999, 01, 01)
            });
            Add(new Game()
            {
                Name = "Grand Theft Auto 3",
                Publisher = "Rockstar Games",
                ReleaseDate = new DateTime(2001, 01, 01)
            });
            Add(new Game()
            {
                Name = "Grand Theft Auto: Vice City",
                Publisher = "Rockstar Games",
                ReleaseDate = new DateTime(2002, 01, 01)
            });
            Add(new Game()
            {
                Name = "Grand Theft Auto: San Andreas",
                Publisher = "Rockstar Games",
                ReleaseDate = new DateTime(2004, 01, 01)
            });
            Add(new Game()
            {
                Name = "Grand Theft Auto: Liberty City Stories",
                Publisher = "Rockstar Games",
                ReleaseDate = new DateTime(2005, 01, 01)
            });
            Add(new Game()
            {
                Name = "Grand Theft Auto IV",
                Publisher = "Rockstar Games",
                ReleaseDate = new DateTime(2008, 01, 01)
            });
            Add(new Game()
            {
                Name = "Grand Theft Auto V",
                Publisher = "Rockstar Games",
                ReleaseDate = new DateTime(2013, 01, 01)
            });

            Add(new Game()
            {
                Name = "Hitman: Codename 47",
                Publisher = "Eidos Interactive",
                ReleaseDate = new DateTime(2000, 01, 01)
            });
            Add(new Game()
            {
                Name = "Hitman 2: Silent Assassin",
                Publisher = "Eidos Interactive",
                ReleaseDate = new DateTime(2002, 01, 01)
            });
            Add(new Game()
            {
                Name = "Hitman: Contracts",
                Publisher = "Eidos Interactive",
                ReleaseDate = new DateTime(2004, 01, 01)
            });
            Add(new Game()
            {
                Name = "Hitman: Blood Money",
                Publisher = "Eidos Interactive",
                ReleaseDate = new DateTime(2006, 01, 01)
            });
            Add(new Game()
            {
                Name = "Hitman: Absolution",
                Publisher = "Square Enix",
                ReleaseDate = new DateTime(2012, 01, 01)
            });
        }

        public static void Add(Game game)
        {
            list.Add(game);
        }

        public static List<Game> GetAll()
        {
            return list;
        }
    }
}