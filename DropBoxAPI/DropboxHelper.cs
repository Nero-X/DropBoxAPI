using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace DropBoxAPI
{
    class DropboxHelper
    {
        /// <summary>
        /// Run tests for user-level operations.
        /// </summary>
        /// <param name="client">The Dropbox client.</param>
        /// <returns>An asynchronous task.</returns>
        public static async Task RunUserTests(DropboxClient client)
        {
            var path = "/DotNetApi/Help";
            var folder = await CreateFolder(client, path);
            var list = await ListFolder(client, path);

            var firstFile = list.Entries.FirstOrDefault(i => i.IsFile);
            if (firstFile != null)
            {
                await Download(client, path, firstFile.AsFile);
            }

            await Upload(client, path, "Test.txt", "This is a text file");

            await ChunkUpload(client, path, "Binary");
        }

        /// <summary>
        /// Creates the specified folder.
        /// </summary>
        /// <remarks>This demonstrates calling an rpc style api in the Files namespace.</remarks>
        /// <param name="path">The path of the folder to create.</param>
        /// <param name="client">The Dropbox client.</param>
        /// <returns>The result from the ListFolderAsync call.</returns>
        public static async Task<FolderMetadata> CreateFolder(DropboxClient client, string path)
        {
            Console.WriteLine("--- Creating Folder ---");
            var folderArg = new CreateFolderArg(path);
            try
            {
                var folder = await client.Files.CreateFolderV2Async(folderArg);

                Console.WriteLine("Folder: " + path + " created!");

                return folder.Metadata;
            }
            catch (ApiException<CreateFolderError> e)
            {
                if (e.Message.StartsWith("path/conflict/folder"))
                {
                    Console.WriteLine("Folder already exists... Skipping create");
                    return null;
                }
                else
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Lists the items within a folder.
        /// </summary>
        /// <remarks>This demonstrates calling an rpc style api in the Files namespace.</remarks>
        /// <param name="path">The path to list.</param>
        /// <param name="client">The Dropbox client.</param>
        /// <returns>The result from the ListFolderAsync call.</returns>
        public static async Task<ListFolderResult> ListFolder(DropboxClient client, string path)
        {
            Console.WriteLine("--- Files ---");
            var list = await client.Files.ListFolderAsync(path);

            // show folders then files
            foreach (var item in list.Entries.Where(i => i.IsFolder))
            {
                Console.WriteLine("D  {0}/", item.Name);
            }

            foreach (var item in list.Entries.Where(i => i.IsFile))
            {
                var file = item.AsFile;

                Console.WriteLine("F{0,8} {1}",
                    file.Size,
                    item.Name);
            }

            if (list.HasMore)
            {
                Console.WriteLine("   ...");
            }
            return list;
        }

        /// <summary>
        /// Downloads a file.
        /// </summary>
        /// <remarks>This demonstrates calling a download style api in the Files namespace.</remarks>
        /// <param name="client">The Dropbox client.</param>
        /// <param name="folder">The folder path in which the file should be found.</param>
        /// <param name="file">The file to download within <paramref name="folder"/>.</param>
        /// <returns></returns>
        public static async Task Download(DropboxClient client, string folder, FileMetadata file)
        {
            Console.WriteLine("Download file...");

            using (var response = await client.Files.DownloadAsync(folder + "/" + file.Name))
            {
                Console.WriteLine("Downloaded {0} Rev {1}", response.Response.Name, response.Response.Rev);
                Console.WriteLine("------------------------------");
                Console.WriteLine(await response.GetContentAsStringAsync());
                Console.WriteLine("------------------------------");
            }
        }

        /// <summary>
        /// Uploads given content to a file in Dropbox.
        /// </summary>
        /// <param name="client">The Dropbox client.</param>
        /// <param name="folder">The folder to upload the file.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="fileContent">The file content.</param>
        /// <returns></returns>
        public static async Task Upload(DropboxClient client, string folder, string fileName, string fileContent)
        {
            Console.WriteLine("Upload file...");

            using (var stream = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(fileContent)))
            {
                var response = await client.Files.UploadAsync(folder + "/" + fileName, WriteMode.Overwrite.Instance, body: stream);

                Console.WriteLine("Uploaded Id {0} Rev {1}", response.Id, response.Rev);
            }
        }

        /// <summary>
        /// Uploads a big file in chunk. The is very helpful for uploading large file in slow network condition
        /// and also enable capability to track upload progerss.
        /// </summary>
        /// <param name="client">The Dropbox client.</param>
        /// <param name="folder">The folder to upload the file.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <returns></returns>
        public static async Task ChunkUpload(DropboxClient client, string folder, string fileName)
        {
            Console.WriteLine("Chunk upload file...");
            // Chunk size is 128KB.
            const int chunkSize = 128 * 1024;

            // Create a random file of 1MB in size.
            var fileContent = new byte[1024 * 1024];
            new Random().NextBytes(fileContent);

            using (var stream = new MemoryStream(fileContent))
            {
                int numChunks = (int)Math.Ceiling((double)stream.Length / chunkSize);

                byte[] buffer = new byte[chunkSize];
                string sessionId = null;

                for (var idx = 0; idx < numChunks; idx++)
                {
                    Console.WriteLine("Start uploading chunk {0}", idx);
                    var byteRead = stream.Read(buffer, 0, chunkSize);

                    using (MemoryStream memStream = new MemoryStream(buffer, 0, byteRead))
                    {
                        if (idx == 0)
                        {
                            var result = await client.Files.UploadSessionStartAsync(body: memStream);
                            sessionId = result.SessionId;
                        }

                        else
                        {
                            UploadSessionCursor cursor = new UploadSessionCursor(sessionId, (ulong)(chunkSize * idx));

                            if (idx == numChunks - 1)
                            {
                                await client.Files.UploadSessionFinishAsync(cursor, new CommitInfo(folder + "/" + fileName), memStream);
                            }

                            else
                            {
                                await client.Files.UploadSessionAppendV2Async(cursor, body: memStream);
                            }
                        }
                    }
                }
            }
        }
    }
}
