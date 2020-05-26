namespace SSO.Services.RequestModel.Client
{
    public class ClientDeviceFlowRequest
    {
        public string UserCodeType { get; set; }
        public int DeviceCodeLifetime { get; set; }
    }
}
