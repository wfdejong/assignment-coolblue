using Insurance.Api.Dto;

namespace Insurance.Api.Infrasctructure
{
    public interface ISurchargeResository
    {
        float Get(string productTypeName);
        void Add(string productTypeName, float value);
    }
}
