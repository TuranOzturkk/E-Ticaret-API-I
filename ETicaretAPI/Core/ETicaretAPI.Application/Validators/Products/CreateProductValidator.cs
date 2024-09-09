using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator: AbstractValidator<VM_Product_Create>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Ürün Adını Boş Girmeyiniz.")
                .MaximumLength(150)
                .MinimumLength(3)
                .WithMessage("Ürün Adını 5 ile 150 Karakter Arasında Giriniz.");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                .WithMessage("Stok Giriniz.")
                .Must(s => s >= 0)
                .WithMessage("Stok Negatif Olamaz!");

            RuleFor(p => p.Price)
               .NotEmpty()
               .NotNull()
               .WithMessage($"Fiyat Giriniz.")
               .Must(s => s >= 0)
               .WithMessage("Fiyat Negatif Olamaz!");
        }
    }
}
