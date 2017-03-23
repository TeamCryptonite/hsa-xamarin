namespace HsaServiceDtos
{
    public class LocationDto
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public bool IsValidLocation()
        {
            if (Latitude == null || Longitude == null)
                return false;
            if (Latitude < -90 || Latitude > 90)
                return false;
            if (Longitude < -180 || Longitude > 180)
                return false;

            return true;
        }
    }
}