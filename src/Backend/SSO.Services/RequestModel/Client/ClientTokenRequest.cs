namespace SSO.Services.RequestModel.Client
{
    public class ClientTokenRequest
    {
        public int IdentityTokenLifetime { get; set; }
        public int AccessTokenLifetime { get; set; }
        public string AccessTokenType { get; set; }
        public int AuthorizationCodeLifetime { get; set; }
        public int AbsoluteRefreshTokenLifetime { get; set; }
        public int SlidingRefreshTokenLifetime { get; set; }
        public string RefreshTokenUsage { get; set; }
        public string RefreshTokenExpiration { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        public bool IncludeJwtId { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public string PairWiseSubjectSalt { get; set; }
        public string ClientClaimsPrefix { get; set; }
    }
}
