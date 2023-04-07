namespace Insurance.Api.Requests
{
    public class SurchargeRequest
    {
        public string ProductTypeName { get; set; }
        public float Surcharge { get; set; }
    }
}
