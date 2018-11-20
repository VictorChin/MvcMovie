using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class UploadFilesController : Controller
    {
        private IConfiguration _config;
        private CloudStorageAccount account;
        private CloudBlobClient blobClient;
        private CloudTableClient tableClient;

        public UploadFilesController(IConfiguration config)
        {
            _config = config;
            account = CloudStorageAccount.Parse(config["SAConnStr"]);
            blobClient= account.CreateCloudBlobClient();
            tableClient = account.CreateCloudTableClient();
        }

        public async Task<IActionResult> Index(int id, List<IFormFile> files)
        {
            var container=blobClient.GetContainerReference("victor");
            await container.CreateIfNotExistsAsync();

            var cloudTable = tableClient.GetTableReference("victormovies");
            await cloudTable.CreateIfNotExistsAsync();

            foreach (var file in files)
            {
                var blob = container.GetBlockBlobReference(file.FileName);
                blob.Metadata["content-type"] = file.ContentType;
                await blob.UploadFromStreamAsync(file.OpenReadStream());
                var poster = new MoviePoster(id, file.FileName, blob.Uri.ToString());
                var insertOp = TableOperation.InsertOrMerge(poster);
                await cloudTable.ExecuteAsync(insertOp);
             }
            return RedirectToAction("Index","Movies");
        }
    }
}