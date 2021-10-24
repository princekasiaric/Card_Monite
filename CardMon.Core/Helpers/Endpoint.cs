namespace CardMon.Core.Helpers
{
    public class Endpoint
    {
        public string Link { get; set; }
        public string Unlink { get; set; }
        public string Resync { get; set; }
        public string Account { get; set; }
        public string User { get; set; }
        public string Validate { get; set; }
        public string UpdateUser { get; set; }
        public string ActivateUser { get; set; }
        public string UnlockUser { get; set; }
        public string LockUser { get; set; }
        public string AddUserAccount { get; set; }
        public string DeleteUserAccount { get; set; }
        public string ResetSecret { get; set; }
        public string Limits { get; set; }
        public string Limit { get; set; }
        public string ResetUserPassword { get; set; }
        public string AssignToken { get; set; }
        public string UnAssignToken { get; set; }
        public string ResyncToken { get; set; }
        public string ExemptCardAuth { get; set; }
        public string Transactions { get; set; }
        public string Audit { get; set; }
        public string Profiles { get; set; }
        public string Roles { get; set; }
        public string SearchOrganization { get; set; }
        public string Corporate { get; set; }
        public string Config { get; set; }
        public string Cache { get; set; }
        public string CacheList { get; set; }
        public string GroupList { get; set; }
        public string CreateOrganization { get; set; }
        public string CreateCorporateUser { get; set; }
        public string CreateRetailUser { get; set; }
        public string PinReset { get; set; }
    }
}
