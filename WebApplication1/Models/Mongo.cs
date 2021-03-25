using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Mongo
    {
        private MongoClient client;
        public IMongoDatabase db;

        public Mongo()
        {
            client = new MongoClient(Environment.GetEnvironmentVariable("connection"));
            db = client.GetDatabase("Web");
        }

    }
}