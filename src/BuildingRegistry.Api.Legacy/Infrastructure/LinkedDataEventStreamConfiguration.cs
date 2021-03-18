namespace BuildingRegistry.Api.Legacy.Infrastructure
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LinkedDataEventStreamConfiguration
    {
        public string DataVlaanderenNamespace { get; set; }
        public string DataVlaanderenBuildingUnitNamespace { get; set; }
        public string ApiEndpoint { get; set; }

        public LinkedDataEventStreamConfiguration(IConfigurationSection configuration)
        {
            DataVlaanderenNamespace = configuration["DatavlaanderenNamespace"];
            DataVlaanderenBuildingUnitNamespace = configuration["DataVlaanderenBuildingUnitNamespace"];
            ApiEndpoint = configuration["ApiEndpoint"];
        }
    }
}
