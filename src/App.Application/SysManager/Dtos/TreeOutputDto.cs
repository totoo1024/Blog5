namespace App.Application.SysManager.Dtos
{
    /// <summary>
    /// 树形节点
    /// </summary>
    public class TreeOutputDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 父级id
        /// </summary>
        public string pid { get; set; }
        /// <summary>
        /// 是否展开
        /// </summary>
        public bool open { get; set; } = true;
    }
}