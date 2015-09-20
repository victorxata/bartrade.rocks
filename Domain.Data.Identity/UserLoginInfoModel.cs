namespace Domain.Data.Identity
{
    // Models returned by AccountController actions.

    /// <summary>
    /// 
    /// </summary>
    public class UserLoginInfoModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProviderKey { get; set; }
    }
}
