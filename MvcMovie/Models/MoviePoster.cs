using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class MoviePoster : TableEntity
    {
        public MoviePoster()
        {

        }
        public MoviePoster(int movieID,string fileName, string URL)
        {
            PartitionKey = movieID.ToString();
            RowKey = fileName;
            this.URL = URL;
        }
        public int MovieID =>int.Parse(PartitionKey);
        public string FilleName => RowKey;
        public string URL { get; set; }
    }
}
