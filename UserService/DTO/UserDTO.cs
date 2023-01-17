namespace UserService.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string? Nom { get; set; }

        public string Email { get; set; }

        public string? ProfilePictureUrl { get; set; }
        
        // Used for unit testing
        public override bool Equals(object? obj)
        {
            if (obj is UserDTO)
            {
                var o = obj as UserDTO;
                return this.UserId == o.UserId 
                    && this.Nom == o.Nom 
                    && this.Email == o.Email 
                    && this.ProfilePictureUrl == o.ProfilePictureUrl;
            }
            return false;
        }
    }
}
