using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordNotes.Models;

namespace WordNotes.Services
{
    public interface IWordLoaderService
    {
        List<Word> LoadWords(AppSettings appSettings);
    }
}
