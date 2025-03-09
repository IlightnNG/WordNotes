using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordNotes.Models
{
    public class AppSettings
    {
        public List<int> FavoriteIndices { get; set; } = new();
        public double MainWindowLeft { get; set; }
        public double MainWindowTop { get; set; }
        public double MenuWindowLeft { get; set; }
        public double MenuWindowTop { get; set; }
        public int TimerIntervalSeconds { get; set; } = 5;
        public string DictionaryPath { get; set; } = "Dictionary/words.xlsx";
    }
}
