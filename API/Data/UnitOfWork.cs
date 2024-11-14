using System;
using API.Interfaces;

namespace API.Data;

public class UnitOfWork (DataContext context, 
        IUserRepository userRepository, ILikeRepository likeRepository, IPhotoRepository photoRepository, 
        IRatingRepository ratingRepository, IRoleRepository roleRepository,
        IRestaurantRepository restaurantRepository, IShippingAddressRepository shippingAddressRepository)
{

}
