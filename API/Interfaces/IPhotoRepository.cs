using API.Entities;

namespace API.Interfaces;

public interface IPhotoRepository
{
    void RemovePhoto(Photo photo);
    Task<Photo?> GetPhotoById(int id);
}
