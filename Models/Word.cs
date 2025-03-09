using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordNotes.Models
{
    public class Word
    {
        public string English { get; set; } // 英文单词
        public string Chinese { get; set; }  // 中文解释
        public bool IsFavorite { get; set; }
    }
}
