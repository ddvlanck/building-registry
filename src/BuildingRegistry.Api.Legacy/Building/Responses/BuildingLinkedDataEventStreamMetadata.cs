namespace BuildingRegistry.Api.Legacy.Building.Responses
{
    using BuildingRegistry.Api.Legacy.Infrastructure;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BuildingLinkedDataEventStreamMetadata
    {
        public static Uri GetPageIdentifier(LinkedDataEventStreamConfiguration configuration, int page)
            => new Uri($"{configuration.BuildingUnitApiEndpoint}?page={page}");

        public static Uri GetCollectionLink(LinkedDataEventStreamConfiguration configuration)
            => new Uri($"{configuration.BuildingUnitApiEndpoint}");

        public static Uri GetShapeUri(LinkedDataEventStreamConfiguration configuration)
            => new Uri($"{configuration.BuildingUnitApiEndpoint}/shape");

        public static List<HypermediaControl>? GetHypermediaControls(
            List<BuildingVersionObject> items,
            LinkedDataEventStreamConfiguration configuration,
            int page,
            int pageSize)
        {
            List<HypermediaControl> controls = new List<HypermediaControl>();

            var previous = AddPrevious(items, configuration, page);
            if (previous != null)
                controls.Add(previous);

            var next = AddNext(items, configuration, page, pageSize);
            if (next != null)
                controls.Add(next);

            return controls.Count > 0 ? controls : null;
        }

        private static HypermediaControl? AddPrevious(
            List<BuildingVersionObject> items,
            LinkedDataEventStreamConfiguration configuration,
            int page)
        {
            if (page <= 1)
                return null;

            var previousUrl = new Uri($"{configuration.BuildingUnitApiEndpoint}?page={page - 1}");

            return new HypermediaControl
            {
                Type = "tree:Relation",
                Node = previousUrl
            };
        }

        private static HypermediaControl? AddNext(
            List<BuildingVersionObject> items,
            LinkedDataEventStreamConfiguration configuration,
            int page,
            int pageSize)
        {
            if (items.Count != pageSize)
                return null;

            var nextUrl = new Uri($"{configuration.ApiEndpoint}?page={page + 1}");

            return new HypermediaControl
            {
                Type = "tree:Relation",
                Node = nextUrl
            };
        }
    }

    public class HypermediaControl
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("tree:node")]
        public Uri Node { get; set; }
    }
}
