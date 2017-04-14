using PhotoGalleryService.Data.Model;

namespace PhotoGalleryService.Features.Accounts
{
    public class AccountApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }

        public static TModel FromAccount<TModel>(Account account) where
            TModel : AccountApiModel, new()
        {
            var model = new TModel();
            model.Id = account.Id;
            model.TenantId = account.TenantId;
            model.Name = account.Name;
            return model;
        }

        public static AccountApiModel FromAccount(Account account)
            => FromAccount<AccountApiModel>(account);

    }
}
