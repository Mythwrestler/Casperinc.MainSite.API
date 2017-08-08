using System;
namespace CasperInc.MainSite.Helpers
{
    public class CommentResourceParameters
    {
        public CommentResourceParameters() { }


        const int maxPageSize = 20;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value >= maxPageSize) ? maxPageSize : value;
            }

        }

    }
}
