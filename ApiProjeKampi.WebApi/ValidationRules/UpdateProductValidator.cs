﻿using ApiProjeKampi.WebApi.Dtos.ProductDtos;
using FluentValidation;

namespace ApiProjeKampi.WebApi.ValidationRules
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Ürün adını boş geçmeyin!");
            RuleFor(x => x.ProductName).MinimumLength(2).WithMessage("En az 2 karakter veri girişi yapın!");
            RuleFor(x => x.ProductName).MaximumLength(50).WithMessage("En fazla 50 karakter veri girişi yapın!");

            RuleFor(x => x.Price).NotEmpty().WithMessage("Ürün fiyatı boş geçilemez!").GreaterThan(0).WithMessage("Ürün fiyatı negatif olamaz!").LessThan(1000).WithMessage("Ürün Fiyatı bu kadar yüksek olamaz girdiğiniz değeri kontrol edin!");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Ürün açıklaması boş geçilemez!");
        }
    }
}
