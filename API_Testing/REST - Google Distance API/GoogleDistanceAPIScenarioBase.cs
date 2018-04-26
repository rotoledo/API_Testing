using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Testing.Google_Distance_API
{
	public class GoogleDistanceAPIScenarioBase
	{
		private const string ApiUrlBase = "https://maps.googleapis.com/maps/api/distancematrix/json";

		public class Get
		{
			public static string GetDistance(GoogleDistanceAPISearchFilter googleDistanceAPISearchFilter)
			{
				return $"{ApiUrlBase}?" +
					$"units={googleDistanceAPISearchFilter.Units}&" +
					$"origins={googleDistanceAPISearchFilter.Origins}&" +
					$"destinations={googleDistanceAPISearchFilter.Destinations}";
			}
		}

		public class Post
		{
			public static string PostDistance()
			{
				return $"{ApiUrlBase}";
			}
		}

		public class GoogleDistanceAPISearchFilter
		{
			public string Origins;
			public string Destinations;
			public string Units; // imperial, metric

			public GoogleDistanceAPISearchFilter() { }

			public GoogleDistanceAPISearchFilter(string origins, string destinations, string units)
			{
				Origins = origins;
				Destinations = destinations;
				Units = units;
			}
		}

	}



}
