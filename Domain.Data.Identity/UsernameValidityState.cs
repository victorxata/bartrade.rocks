namespace Domain.Data.Identity
{
    /// <summary>
    /// When querying the server to check for a valid and/or taken username,
    /// these are the possible results.
    /// </summary>
    public enum UsernameValidityState
    {
        Ok,
        Taken,
        Invalid,
        Empty
    }
}
