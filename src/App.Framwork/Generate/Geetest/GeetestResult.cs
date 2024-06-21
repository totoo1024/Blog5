namespace App.Framwork.Generate.Geetest
{
    public class GeetestResult
    {
        public GeetestResult()
        {

        }

        public GeetestResult(string data)
        {
            this.data = data;
        }
        /// <summary>
        /// 成功失败的标识码，1表示成功，0表示失败
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 备注信息，如异常信息等
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 返回数据，json格式
        /// </summary>
        public string data { get; set; }
    }
}