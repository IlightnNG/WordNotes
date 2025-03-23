using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WordNotes.Models;

namespace WordNotes.Services
{
    public class WordQueueService
    {
        private List<Word> allWords;
        // 展示队列
        public List<int> historyQueue = new List<int>();
        // 记忆队列
        public List<int> favoriteQueue = new List<int>();
        // 前一次新单词
        public List<int> lastNewWordsQueue = new List<int>();
        public List<int> newWordsQueue = new List<int>();
        // 前一次复习单词
        public List<int> lastReviewWordsQueue = new List<int>();
        public List<int> reviewWordsQueue = new List<int>();


        private AppSettings _appSettings;
        private SettingsService _settingsService;
        // 今日已出现的新词和复习词个数
        public int newWordsNum = 0;
        public int reviewWordsNum = 0;

        // 随机数生成器
        private Random random = new Random();

        public WordQueueService(AppSettings appSettings, List<Word> words,SettingsService settingsService)
        {
            _appSettings = appSettings;
            _settingsService = settingsService;
            allWords = words;
            favoriteQueue = _appSettings.FavoriteIndices;
            // 如果上次登录时间并非今天，将之前的当天记录作为今天的上一次记录
            if(_appSettings.LastAccessDate< DateTime.Now.Date)
            {
                
                _appSettings.LastReviewWordsIndices = new List<int>(_appSettings.LastReviewWordsIndices);
                _appSettings.LastReviewWordsIndices.AddRange(_appSettings.ReviewWordsIndices);
                _appSettings.LastReviewWordsIndices.AddRange(_appSettings.LastNewWordsIndices);

                if (_appSettings.LastReviewWordsIndices.Count > 200)
                {
                    // 删除200之后的所有元素
                    _appSettings.LastReviewWordsIndices.RemoveRange(200, _appSettings.LastReviewWordsIndices.Count - 200);
                }

                _appSettings.LastNewWordsIndices = new List<int>(_appSettings.NewWordsIndices);
                _appSettings.NewWordsIndices = new List<int>();
                _appSettings.ReviewWordsIndices = new List<int>();
                settingsService.SaveSettings(appSettings);
            }

            

            lastNewWordsQueue = _appSettings.LastNewWordsIndices;
            newWordsQueue = _appSettings.NewWordsIndices;
            lastReviewWordsQueue = _appSettings.LastReviewWordsIndices;
            reviewWordsQueue = _appSettings.ReviewWordsIndices;

            newWordsNum = _appSettings.NewWordsIndices.Count;
            reviewWordsNum = _appSettings.ReviewWordsIndices.Count;
            historyQueue = new List<int>(_appSettings.NewWordsIndices);
            historyQueue.AddRange(_appSettings.ReviewWordsIndices);
        }

        public int NextWord()
        {
            int index = 0;
            if (favoriteQueue.Count > 0 && random.Next(100) <= _appSettings.FavoriteQueueProbability) // 进入记忆队列概率
            {
                // 从队列中随机获取一个单词
                index = favoriteQueue[random.Next(favoriteQueue.Count)];
            }
            // 每日单词模式未开启，随机出现单词
            else if (!_appSettings.IsDailyWordsMode)
            {
                index = random.Next(allWords.Count);
            }
            // 概率进入复习
            else if ( random.Next(_appSettings.ReviewWordsNum/_appSettings.NewWordsNum) != 0 && lastNewWordsQueue != null && lastReviewWordsQueue != null && reviewWordsNum < _appSettings.ReviewWordsNum && (lastNewWordsQueue.Count != 0 || lastReviewWordsQueue.Count != 0))
            {
                // 复习单词从上一次的单词
                // 记录是否有未复习单词，能否复习
                bool ifReview = true;
                if (lastNewWordsQueue.Count != 0 && random.Next(2) == 0)
                {
                    do
                    {
                        index = lastNewWordsQueue[random.Next(lastNewWordsQueue.Count)];
                    } while (historyQueue.Contains(index));
                    lastNewWordsQueue.Remove(index);
                }
                else if (lastReviewWordsQueue.Count != 0)
                {
                    do
                    {
                        index = lastReviewWordsQueue[random.Next(lastReviewWordsQueue.Count)];
                    } while (historyQueue.Contains(index));
                    lastReviewWordsQueue.Remove(index);
                }
                else if (lastNewWordsQueue.Count != 0)
                {
                    do
                    {
                        index = lastNewWordsQueue[random.Next(lastNewWordsQueue.Count)];
                    } while (historyQueue.Contains(index));
                    lastNewWordsQueue.Remove(index);
                }
                else ifReview = false;

                if (ifReview)
                {
                    reviewWordsNum++;
                    reviewWordsQueue.Add(index);
                }else
                {
                    index = historyQueue[random.Next(historyQueue.Count)];
                }
                
            }
            else if (newWordsNum < _appSettings.NewWordsNum ) 
            {
                // 生成新词，保证不在上一次的新词和复习单词中
                index = random.Next(allWords.Count);
                if (!historyQueue.Contains(index))
                {
                    newWordsNum++;
                    newWordsQueue.Add(index);
                }
            }
            else
            {
                // 循环历史列表中的词
                index = historyQueue[random.Next(historyQueue.Count)];
            }

            if (historyQueue.Contains(index)) historyQueue.Remove(index);
            historyQueue.Add(index);

            _appSettings.LastAccessDate = DateTime.Now.Date;
            _settingsService.SaveSettings(_appSettings);
            return index;
        }
    }
}
