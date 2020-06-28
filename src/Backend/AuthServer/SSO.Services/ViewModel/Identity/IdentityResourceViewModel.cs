namespace SSO.Services.ViewModel.Identity
{
    public class IdentityResourceViewModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
    }
}
