using iChave.Validator;

namespace iChave.ModelDTO
{
    public class ServiceDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double low_price { get; set; }
        public double high_price { get; set; }

        public ServiceDTO()
        {
        }
        public ServiceDTO(ServiceVal _serviceValidator)
        {
            name = _serviceValidator.name;
            description = _serviceValidator.description;
            low_price = _serviceValidator.low_price;
            high_price = _serviceValidator.high_price;
        }
    }
}
