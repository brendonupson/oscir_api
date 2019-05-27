using System;
namespace OSCiR.Model
{
    public class DataSetPager
    {
        const int DEFAULT_PAGE_SIZE = 100;
        const int ABSOLUTE_MAX_PAGE_SIZE = 1000;

        public int TotalRecordCount { get; set; } //Total number of results for entire query

        private int _startRowIndex;
        public int StartRowIndex
        {
            get{ return _startRowIndex; }
            set
            {
                _startRowIndex = value;
                if (value < 0) _startRowIndex = 0; //ensure >=0
            }
        } //Where the current page should start from, zero based

        private int _maxPageSize;
        public int MaxPageSize {
            get { return _maxPageSize;  }
            set {
                //so users cannot request stupid large size, or <= zero
                _maxPageSize = value;
                if (_maxPageSize <= 0) _maxPageSize = DEFAULT_PAGE_SIZE;
                if (_maxPageSize > ABSOLUTE_MAX_PAGE_SIZE) _maxPageSize = ABSOLUTE_MAX_PAGE_SIZE;
            } } //Max number of rows to return
        public int CurrentResultCount { get; set; } //Number of rows actually returned


        public DataSetPager()
        {
            TotalRecordCount = 0;
            StartRowIndex = 0;
            MaxPageSize = DEFAULT_PAGE_SIZE;
            CurrentResultCount = 0;
        }
    }
}
