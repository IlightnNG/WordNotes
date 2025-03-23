using System.ComponentModel;


namespace WordNotes.Models
{
    public class ConfigInSettingsPage : INotifyPropertyChanged
    {
        public int _timerIntervalSeconds = 1;
        public bool _isDailyWordsMode = false;
        public int _newWordsNum = 0;
        public int _reviewWordsNum = 0;
        public string _dictionaryPath = "";
        public int _favoriteQueueProbability = 20;

        public int TimerIntervalSeconds
        {
            get => _timerIntervalSeconds;
            set
            {
                if (_timerIntervalSeconds != value)
                {
                    _timerIntervalSeconds = value;
                    OnPropertyChanged(nameof(TimerIntervalSeconds)); // 触发属性更改通知
                }
            }
        }
        public int NewWordsNum
        {
            get => _newWordsNum;
            set
            {
                if (_newWordsNum != value)
                {
                    _newWordsNum = value;
                    OnPropertyChanged(nameof(NewWordsNum)); // 触发属性更改通知
                }
            }
        }
        public int ReviewWordsNum
        {
            get => _reviewWordsNum;
            set
            {
                if (_reviewWordsNum != value)
                {
                    _reviewWordsNum = value;
                    OnPropertyChanged(nameof(ReviewWordsNum)); // 触发属性更改通知
                }
            }
        }
        public string DictionaryPath
        {
            get => _dictionaryPath;
            set
            {
                if (_dictionaryPath != value)
                {
                    _dictionaryPath = value;
                    OnPropertyChanged(nameof(DictionaryPath)); // 触发属性更改通知
                }
            }
        }
        public int FavoriteQueueProbability
        {
            get => _favoriteQueueProbability;
            set
            {
                if (_favoriteQueueProbability != value)
                {
                    _favoriteQueueProbability = value;
                    OnPropertyChanged(nameof(FavoriteQueueProbability)); // 触发属性更改通知
                }
            }
        }
        public bool IsDailyWordsMode
        {
            get => _isDailyWordsMode;
            set
            {
                if (_isDailyWordsMode != value)
                {
                    _isDailyWordsMode = value;
                    OnPropertyChanged(nameof(IsDailyWordsMode)); // 触发属性更改通知
                }
            }
        }


        public ConfigInSettingsPage(AppSettings appSettings)
        {
            TimerIntervalSeconds = appSettings.TimerIntervalSeconds;
            IsDailyWordsMode = appSettings.IsDailyWordsMode;
            NewWordsNum = appSettings.NewWordsNum;
            ReviewWordsNum = appSettings.ReviewWordsNum;
            DictionaryPath = appSettings.DictionaryPath;
            FavoriteQueueProbability = appSettings.FavoriteQueueProbability;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
