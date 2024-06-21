namespace App.Application
{
    public class PageInputDto
    {
        private int _page = 1;

        private int _limit = 10;

        /// <summary>
        /// 当前页
        /// </summary>
        public int Page
        {
            get => _page;
            set => _page = value > 0 ? value : 1;
        }

        /// <summary>
        /// 每页显示的数量
        /// </summary>
        public int Limit
        {
            get => _limit;
            set => _limit = value > 0 ? value : 10;
        }
    }
}