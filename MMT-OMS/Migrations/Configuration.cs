namespace MMT_OMS.Migrations
{
    using MMT_OMS.Models;
    using MMT_OMS.Services;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Text;
    using Enums;

    internal sealed class Configuration : DbMigrationsConfiguration<MMT_OMS.MMTModel>
    {
        public Configuration()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MMTModel>());
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MMTModel context)
        {
            // Order Status
            var orderStatuses = new List<CustomerOrderStatus>()
            {
                new CustomerOrderStatus
                {
                    Id = 1,
                    Name = OrderStatusName.Payment.ToString()
                },
                new CustomerOrderStatus
                {
                    Id = 2,
                    Name = OrderStatusName.LabelPrinting.ToString()
                },
                new CustomerOrderStatus
                {
                    Id = 3,
                    Name = OrderStatusName.LabelApplication.ToString()
                },
                new CustomerOrderStatus
                {
                    Id = 4,
                    Name = OrderStatusName.Packaging.ToString()
                },
                new CustomerOrderStatus
                {
                    Id = 5,
                    Name = OrderStatusName.Shipping.ToString()
                },
                new CustomerOrderStatus
                {
                    Id = 6,
                    Name = OrderStatusName.Done.ToString()
                }
            };

            context.CustomerOrderStatuses.AddRange(orderStatuses);

            // Tint Types and Shades
            var matteShadeNames = new string[]
            {
                "Rosewood",
                "Bubblegum",
                "Darkred",
                "Tawny",
                "Bloody M",
                "Old Rose",
                "Ginger",
            };
            var gelShadeNames = new string[]
            {
                "Daredevil",
                "Ladyboss",
                "Cheerleader",
                "Beauty queen",
                "Dolly pink",
                "Beach bum",
                "Wildflower",
                "Seductress",
                "Trendsetter",
                "Wander Lust",
            };
            var gelTintType = new TintType
            {
                Id = 0,
                Name = "Gel Lip & Cheek Tint",
                Price = 20,
                Shades = GenerateShades(gelShadeNames)
            };
            var matteTintType = new TintType
            {
                Id = 1,
                Name = "Matte Powdery",
                Price = 25,
                Shades = GenerateShades(matteShadeNames)
            };
            List<TintType> tintTypes = new List<TintType>
            {
                matteTintType,
                gelTintType
            };
            context.TintTypes.AddRange(tintTypes);

            // Bundles
            List<Bundle> bundles = new List<Bundle>()
            {
                new Bundle
                {
                    Code = "40M",
                    Description = "40 Matte Powdery Tints",
                    BundleTintTypes = new List<BundleTintType>()
                    {
                        new BundleTintType
                        {
                            Quantity = 40,
                            TintType =  matteTintType
                        }
                    },
                    Amount = 1000
                },
                new Bundle
                {
                    Code = "50G",
                    Description = "50 Gel Lip and Cheek Tints",
                    BundleTintTypes = new List<BundleTintType>()
                    {
                        new BundleTintType
                        {
                            Id = new Guid(),
                            Quantity = 50,
                            TintType = gelTintType
                        }
                    },
                    Amount = 1000
                },
                new Bundle
                {
                    Code = "25G40M",
                    Description = "25 Gel Lip and Cheek Tints | 40 Matte Powdery Tints",
                    BundleTintTypes = new List<BundleTintType>()
                    {
                        new BundleTintType
                        {
                            Id = new Guid(),
                            Quantity = 25,
                            TintType = gelTintType
                        },
                        new BundleTintType
                        {
                            Id = new Guid(),
                            Quantity = 40,
                            TintType = matteTintType
                        },
                    },
                    Amount = 1500
                },
                new Bundle
                {
                    Code = "50G20M",
                    Description = "50 Gel Lip and Cheek Tints | 20 Matte Powdery Tints",
                    BundleTintTypes = new List<BundleTintType>()
                    {
                        new BundleTintType
                        {
                            Id = new Guid(),
                            Quantity = 50,
                            TintType = gelTintType
                        },
                        new BundleTintType
                        {
                            Id = new Guid(),
                            Quantity = 20,
                            TintType = matteTintType
                        },
                    },
                    Amount = 1500
                },
                new Bundle
                {
                    Code = "25G20M",
                    Description = "25 Gel Lip and Cheek Tints | 20 Matte Powdery Tints",
                    BundleTintTypes = new List<BundleTintType>()
                    {
                        new BundleTintType
                        {
                            Id = new Guid(),
                            Quantity = 25,
                            TintType = gelTintType
                        },
                        new BundleTintType
                        {
                            Id = new Guid(),
                            Quantity = 20,
                            TintType = matteTintType
                        }
                    },
                    Amount = 1000
                }
            };
            bundles.ForEach(bundle => context.Bundles.AddOrUpdate(bundle));

            #region User
            var user = new User
            {
                Username = "admin",
                Salt = Guid.NewGuid()
            };

            var hash = SecurityService.Hash("maureenmae", Encoding.UTF8.GetBytes(user.Salt.ToString()));
            user.PasswordHash = hash;

            context.Users.Add(user);
            #endregion

            context.SaveChanges();
        }

        private List<Shade> GenerateShades(string[] shadeNames)
        {
            var shades = new List<Shade>();
            foreach (string shadeName in shadeNames)
            {
                shades.Add(new Shade(shadeName));
            }

            return shades;
        }

    }
}
