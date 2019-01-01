namespace CarRentalSystem.Infrastructure
{
    using System.ComponentModel.DataAnnotations;

    public class ImageUrlAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var imageUrl = value as string;

            if (imageUrl == null)
            {
                return true;
            }

            return imageUrl.EndsWith(".jpg") ||
                   imageUrl.EndsWith(".jpeg") ||
                   imageUrl.EndsWith(".png") ||
                   imageUrl.EndsWith(".gif");
        }
    }
}