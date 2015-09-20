namespace Domain.Data.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class UserInfoModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasRegistered { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LoginProvider { get; set; }
    }
}