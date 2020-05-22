using System;

namespace SSO.Services.ViewModel.Client
{
    public class ClientSecretsViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }
        public int ClientId { get; set; }
    }
}
