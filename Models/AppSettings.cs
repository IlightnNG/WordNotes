using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordNotes.Models
{
    public class AppSettings
    {
        public double MainWindowLeft { get; set; }
        public double MainWindowTop { get; set; }
        public double MenuWindowLeft { get; set; }
        public double MenuWindowTop { get; set; }
        public int TimerIntervalSeconds { get; set; } = 5;
        public int FavoriteQueueProbability { get; set; } = 20;
        public string DictionaryPath { get; set; } = "Dictionary/words.xlsx";
        public bool IsDailyWordsMode { get; set; } = false;
        public int NewWordsNum { get; set; } = 25;
        public int ReviewWordsNum { get; set; } = 75;
        public List<int> FavoriteIndices { get; set; } = new();
        public List<int> LastNewWordsIndices { get; set; } = new();
        public List<int> NewWordsIndices { get; set; } = new();
        public List<int> LastReviewWordsIndices { get; set; } = new();
        public List<int> ReviewWordsIndices { get; set; } = new();
        public DateTime LastAccessDate { get; set; }
    }
}
