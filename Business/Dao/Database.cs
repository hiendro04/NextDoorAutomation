﻿using MongoDB.Driver;

namespace Business.Dao
{
    public class Database
    {
        private static MongoClient client;
        public static IMongoDatabase GetDatabase()
        {
            if (client == null)
            {
                var mongoUrl = "mongodb://localhost:27017/NextDoorAutomation";
                //var mongoUrl = "mongodb+srv://baclinh0123:0Un9DIlLGijdhneY@linhdev.md6go.mongodb.net/?retryWrites=true&w=majority&appName=linhdev";
                //var mongoUrl = BusinessSettings.MongoDBConnectionStrings;
                client = new MongoClient(mongoUrl);
            }

            return client.GetDatabase("NextDoorAutomation");
        }
    }
}
