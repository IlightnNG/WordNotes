using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordNotes.Models;
using CsvHelper;
using OfficeOpenXml;
using System.Diagnostics;

namespace WordNotes.Services
{
    public class DictionaryLoaderService : IDictinoaryLoaderService
    {
        private AppSettings _appSettings;
        private string filePath;

        public List<Word> LoadWords(AppSettings appSettings)
        {
            

            if(appSettings != null)
            {
                _appSettings = appSettings;
                filePath = _appSettings.DictionaryPath;
                
            }

            if (!File.Exists(filePath))
            {
                throw new InvalidOperationException("文件不存在。");
            }

            if (filePath.EndsWith(".csv"))
            {
                return LoadWordsFromCsv(filePath);
            }
            else if (filePath.EndsWith(".xlsx"))
            {
                return LoadWordsFromExcel(filePath);
            }
            else
            {
                throw new NotSupportedException("Unsupported file format.");
            }
        }

        private List<Word> LoadWordsFromCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Word>().ToList();
            }
        }

        private List<Word> LoadWordsFromExcel(string filePath)
        {
            List<Word> words = new List<Word>();

            // 设置 EPPlus 的 LicenseContext
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // 检查工作簿中是否有工作表
                if (package.Workbook.Worksheets.Count == 0)
                {
                    throw new InvalidOperationException("Excel 文件中没有工作表。");
                }
                var worksheet = package.Workbook.Worksheets[0]; // 读取第一个工作表

                // 检查工作表是否为空
                if (worksheet.Dimension == null)
                {
                    throw new InvalidOperationException("工作表为空。");
                }

                // 获取行数
                //int rowCount = worksheet.Dimension.Rows;
                int rowCount = 1;
                while (worksheet.Cells[rowCount, 1].Value!=null)
                {
                    rowCount++;
                }
                rowCount--;

                for (int row = 1; row <= rowCount; row++) // 假设无标题行
                {
                    words.Add(new Word
                    {
                        English = worksheet.Cells[row, 1].Text, // 第一列是英文
                        Chinese = worksheet.Cells[row, 2].Text,  // 第二列是中文
                        IsFavorite = _appSettings.FavoriteIndices.Contains(row-1) // 是否被收藏
                    });
                }
            }

            return words;
        }
    }
}
