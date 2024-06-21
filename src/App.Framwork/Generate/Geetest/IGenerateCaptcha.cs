namespace App.Framwork.Generate.Geetest
{
    public interface IGenerateValidate
    {
        /// <summary>
        /// 生成验证
        /// </summary>
        /// <returns></returns>
        string Generate();

        /// <summary>
        /// 校验验证
        /// </summary>
        /// <returns></returns>
        bool Validate();
    }
}