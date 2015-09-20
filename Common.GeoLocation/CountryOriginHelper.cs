using System;
using NGeoIP;
using NGeoIP.Client;

namespace Common.GeoLocation
{
    public static class CountryOriginHelper
    {
        public static string RetrieveCountryId(string ip)
        {
            try
            {
                var nGeoRequest = new Request
                {
                    Format = Format.Json,
                    IP = ip
                };

                var nGeoClient = new NGeoClient(nGeoRequest);

                var rawData = nGeoClient.Execute();

                return rawData.CountryCode;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
