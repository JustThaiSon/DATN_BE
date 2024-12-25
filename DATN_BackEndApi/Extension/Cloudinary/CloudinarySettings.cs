using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace DATN_BackEndApi.Extension.CloudinarySett
{
    public class CloudinarySettings
    {
        public string CloudName { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
    public class CloudService
    {
        private readonly Cloudinary _cloudinary;

        public CloudService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        /// <summary>
        /// Tải ảnh lên cloud (hiện tại là cloud của nghĩa).
        /// Giới hạn ảnh 2mb
        /// </summary>
        /// <param name="file"></param>
        /// <returns>
        /// Trả về URL của ẢNH được lưu trên cloud (phục vụ việc lưu vào DB)
        /// </returns>
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.Name, stream)
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }

        /// <summary>
        /// Tải video lên cloud
        /// </summary>
        /// <param name="file"></param>
        /// <returns>
        /// Trả về URL của VIDEO được lưu trên cloud (phục vụ việc lưu vào DB)
        /// </returns>
        public async Task<string> UploadVideoAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(file.Name, stream)
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }


    }
}
