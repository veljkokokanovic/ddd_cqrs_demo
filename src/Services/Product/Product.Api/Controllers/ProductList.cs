﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.Api.Models;

namespace Product.Api.Controllers
{
    public static class ProductList
    {
        public static IEnumerable<ProductViewModel> All
        {
            get
            {
                yield return new ProductViewModel("pz1", "Margarita", Categories.Pizza, 9.95M);
                yield return new ProductViewModel("pz2", "Sizzler", Categories.Pizza, 9.95M);
                yield return new ProductViewModel("pz3", "Texas BBQ", Categories.Pizza, 10.95M);
                yield return new ProductViewModel("pz4", "Diavolo", Categories.Pizza, 10.95M);
                yield return new ProductViewModel("pz5", "Napoli", Categories.Pizza, 9.95M);
                yield return new ProductViewModel("pz6", "Hawaiian", Categories.Pizza, 9.95M);
                yield return new ProductViewModel("dr1", "Coke", Categories.Drink, 1.15M);
                yield return new ProductViewModel("dr2", "Pepsi", Categories.Drink, 1.05M);
                yield return new ProductViewModel("dr3", "Fanta", Categories.Drink, 1.15M);
                yield return new ProductViewModel("dr4", "7Up", Categories.Drink, 1.05M);
            }
        }
    }

    public static class Categories
    {
        public const string Pizza = nameof(Pizza);

        public const string Drink = nameof(Drink);

        public static IEnumerable<string> All
        {
            get
            {
                yield return Pizza;
                yield return Drink;
            }
        }
    }
}
