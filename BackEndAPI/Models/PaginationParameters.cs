namespace BackEndAPI.Models
{
    public class PaginationParameters
    {
        private int _maxPageSize = 50;
        private int _minPageSize = 10;
        private int _pageSize = 10;
        private int _pageNumber = 1;

        public int PageNumber
        {
            get
            {
                return _pageNumber;
            }
            set
            {
                _pageNumber = (value <= 0) ? 1 : value;
            }
        }
        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                _pageSize = (value > _maxPageSize) ? _maxPageSize
                    : (value < _minPageSize) ? _minPageSize
                    : value;
            }
        }
    }
}