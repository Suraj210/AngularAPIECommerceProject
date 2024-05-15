﻿using ECommerceAPI.Application.Services;
using ECommerceAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {

                // todo log
                throw ex;
            }

        }

        async Task<string> FileRenameAsync(string path, string fileName, bool first = true)
        {
            string newFileName = await Task.Run<string>(async () =>
              {
                  string extension = Path.GetExtension(fileName);


                  string newFileName = string.Empty;

                  if (first)
                  {
                      string oldFileName = Path.GetFileNameWithoutExtension(fileName);
                      newFileName = $"{NameOperation.CharacterRegulatory(oldFileName)}{extension}";
                  }
                  else
                  {

                      newFileName = fileName;
                      int indexNo1 = newFileName.IndexOf('-');

                      if (indexNo1 == -1)
                      {
                          newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                      }
                      else
                      {
                          int indexNo2 = newFileName.IndexOf(".");
                          string fileNo = newFileName.Substring(indexNo1, indexNo2 - indexNo1 - 1);
                          int _fileNo = int.Parse(fileNo);
                          _fileNo++;

                          newFileName = newFileName.Remove(indexNo1, indexNo2 - indexNo1 - 1)
                                                   .Insert(indexNo1, _fileNo.ToString());

                      }

                  }


                  if (File.Exists($"{path}\\{newFileName}"))
                      return await FileRenameAsync(path, newFileName, false);
                  else
                      return newFileName;
              });

            return "";

        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();
            List<bool> results = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(uploadPath, file.FileName);

                bool result = await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
                datas.Add((fileNewName, $"{uploadPath}\\{fileNewName}"));
                results.Add(result);
            }

            if (results.TrueForAll(r => r.Equals(true)))
                return datas;
            return null;

            //todo If condition abouve is not true exception should be thrown!

        }
    }
}
