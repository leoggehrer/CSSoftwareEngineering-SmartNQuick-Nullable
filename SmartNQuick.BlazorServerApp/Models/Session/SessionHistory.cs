//@BaseCode
//MdStart
namespace SmartNQuick.BlazorServerApp.Models.Modules.Session
{
    [Serializable]
    public partial class SessionHistory
    {
        private const int MaxSize = 100;
        public List<string> HistoryList { get; private set; }
        public DateTime LastAccessDate { get; private set; }

        public SessionHistory()
        {
            HistoryList = new List<string>() { "/" };
            LastAccessDate = DateTime.Now;
        }

        public bool HasHistory => HistoryList.Any();
        public void ClearHistory()
        {
            LastAccessDate = DateTime.Now;
            HistoryList.Clear();
        }
        public void PushHistory(string entry)
        {
            entry.CheckArgument(nameof(entry));

            LastAccessDate = DateTime.Now;
            HistoryList.Add(entry);
            if (HistoryList.Count > MaxSize)
            {
                HistoryList.RemoveAt(0);
            }
        }
        public string? PopHistory()
        {
            var result = default(string);
            var index = HistoryList.Any() ? HistoryList.Count - 1 : -1;

            LastAccessDate = DateTime.Now;
            if (index > -1)
            {
                result = HistoryList[index];
                HistoryList.RemoveAt(index);
            }
            return result;
        }
        public string? PeekHistory()
        {
            var result = default(string);
            var index = HistoryList.Any() ? HistoryList.Count - 1 : -1;

            LastAccessDate = DateTime.Now;
            if (index > -1)
            {
                result = HistoryList[index];
            }
            return result;
        }
    }
}
//MdEnd