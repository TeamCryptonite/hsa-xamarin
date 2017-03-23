namespace HsaServiceDtos
{
    public class UserDto
    {
        public string UserGuid { get; set; }
        public string EmailAddress { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public bool? IsEmployee { get; set; }
        public bool? IsActiveUser { get; set; }
    }
}