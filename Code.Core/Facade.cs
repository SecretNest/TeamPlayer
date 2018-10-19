using Newtonsoft.Json;
using SecretNest.TeamPlayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.TeamPlayer
{
    public partial class Facade
    {
        DataFile dataFile;
        string fileName;

        public Facade(string fileName)
        {
            this.fileName = fileName;
            if (System.IO.File.Exists(fileName))
            {
                var data = System.IO.File.ReadAllText(fileName);
                dataFile = JsonConvert.DeserializeObject<DataFile>(data);
            }
            else
            {
                dataFile = new DataFile();
                dataFile.Basis = new Basis();
                dataFile.Basis.Races = new Dictionary<Guid, Race>();
                dataFile.Basis.Maps = new Dictionary<Guid, Map>();
                dataFile.Teams = new Dictionary<TeamSelection, Team>();
                dataFile.Teams[TeamSelection.Team1] = new Team() { Players = new Dictionary<Guid, Player>() };
                dataFile.Teams[TeamSelection.Team2] = new Team() { Players = new Dictionary<Guid, Player>() };
                //dataFile.Games = new List<List<Game>>();
            }
        }

        void Save()
        {
            var data = JsonConvert.SerializeObject(dataFile);
            System.IO.File.WriteAllText(fileName, data);
        }
    }
}
