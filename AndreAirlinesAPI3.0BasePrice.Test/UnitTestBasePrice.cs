using AndreAirlinesAPI3._0BasePrice.Service;
using AndreAirlinesAPI3._0BasePrice.Utils;
using AndreAirlinesAPI3._0Models;
using System;
using Xunit;

namespace AndreAirlinesAPI3._0BasePrice.Test
{
    public class UnitTestBasePrice
    {
        private BasePriceService InitializeDataBase()
        {
            var settings = new AndreAirlinesDatabaseBasePriceSettings();
            BasePriceService basePriceService = new(settings);
            return basePriceService;
        }

        [Fact]
        public void GetAll()
        {
            var basePriceService = InitializeDataBase();
            var basePrices = basePriceService.Get();

            var status = basePrices.Count > 0;
            Assert.Equal(true, status);
        }

        [Fact]
        public void GetOne()
        {
            var basePriceService = InitializeDataBase();
            var basePrice = basePriceService.Get("6252d2266f7859cb1488f5ee");
            if (basePrice == null) basePrice = new BasePrice();
            Assert.Equal("6252d2266f7859cb1488f5ee", basePrice.Id);
        }

        [Fact]
        public async void Create()
        {
            BasePrice basePrice = new BasePrice { Destiny = new Airport { IataCode = "POA" }, Origin = new Airport { IataCode = "GRU" }, ErrorCode = "cadastro", LoginUser = "m1m1" };

            var basePriceService = InitializeDataBase();
            var returnMsg = await basePriceService.Create(basePrice, "m1m1", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im0xbTEiLCJyb2xlIjoiYWRtIiwibmJmIjoxNjQ5ODcyMjkwLCJleHAiOjE2NDk4NzU4OTAsImlhdCI6MTY0OTg3MjI5MH0.gIQ055l7Qz7gNyoXeaqE_DvagqttkPBRScFz3GZ70O4");
            Assert.Equal("cadastro", returnMsg.ErrorCode);
        }

        [Fact]
        public async void Update()
        {
            BasePrice flight = new BasePrice { Id = "625714be1b49c5d007f5081c", ErrorCode = "passou", LoginUser = "m1m1" };

            var basePriceService = InitializeDataBase();
            var returnMsg = await basePriceService.Update("625714be1b49c5d007f5081c", flight, "m1m1", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im0xbTEiLCJyb2xlIjoiYWRtIiwibmJmIjoxNjQ5ODcyMjkwLCJleHAiOjE2NDk4NzU4OTAsImlhdCI6MTY0OTg3MjI5MH0.gIQ055l7Qz7gNyoXeaqE_DvagqttkPBRScFz3GZ70O4");
            Assert.Equal("ok", returnMsg);
        }
        [Fact]
        public async void Remove()
        {
            BasePrice flight = new BasePrice { Id = "625714be1b49c5d007f5081c" };

            var basePriceService = InitializeDataBase();
            var returnMsg = await basePriceService.Remove("625714be1b49c5d007f5081c", flight, "m1m1", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im0xbTEiLCJyb2xlIjoiYWRtIiwibmJmIjoxNjQ5ODcyMjkwLCJleHAiOjE2NDk4NzU4OTAsImlhdCI6MTY0OTg3MjI5MH0.gIQ055l7Qz7gNyoXeaqE_DvagqttkPBRScFz3GZ70O4");
            Assert.Equal("ok", returnMsg);
        }
    }
}
