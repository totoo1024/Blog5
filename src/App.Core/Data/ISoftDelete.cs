namespace App.Core.Data
{
    /// <summary>
    /// 软删除
    /// </summary>
    public interface ISoftDelete
    {
        bool DeleteMark { get; set; }
    }
}