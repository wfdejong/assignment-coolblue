using Insurance.Api.Dto;
using System.Collections.Generic;

namespace Insurance.Api.Infrasctructure
{
    public class SurchargeRepository : ISurchargeResository
    {
        private readonly Dictionary<string, float> _surcharges = new Dictionary<string, float>();

        public float Get(string productTypeName) => _surcharges.ContainsKey(productTypeName) ? _surcharges[productTypeName] : 0;

        public void Add(string productTypeName, float value) 
        {
            _surcharges[productTypeName] = value;
        }
    }
}
