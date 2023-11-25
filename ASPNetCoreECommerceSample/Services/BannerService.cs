using ASPNetCoreECommerceSample.Data;
using ASPNetCoreECommerceSample.Entities;

namespace ASPNetCoreECommerceSample.Services
{
    public interface IBannerService
    {
        List<Banner> GetAllBanners();
    }
    public class BannerService : IBannerService
    {
        private readonly ECommerceContext _context;
        public BannerService(ECommerceContext context)
        {
            _context = context;
        }

        public List<Banner> GetAllBanners()
        {
            var bannerList = _context.Banners
                    .OrderBy(p => p.Id)
                    .ToList();

            bannerList = bannerList.Select(b =>
            {
                b.BannerImages = _context.BannerImages.Where(pi => pi.BannerId == b.Id).ToList();
                return b;
            }).ToList();



            return bannerList;
            // return Enumerable.Empty<Banner>().ToList();
        }

    }
}
