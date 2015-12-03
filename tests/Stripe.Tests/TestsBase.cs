using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Stripe;
using ServiceStack.Stripe.Types;

namespace Stripe.Tests
{
    public class TestsBase
    {
        const string LicenseTextPath = @"C:\users\johann\development\ServiceStack.license.txt";
        protected readonly StripeGateway gateway = new StripeGateway("sk_test_23KlmQohLKD4dfmAvxYESZ2z");

        public TestsBase()
        {
            if (File.Exists(LicenseTextPath))
                Licensing.RegisterLicenseFromFile(LicenseTextPath);
        }

        protected StripeCustomer CreateCustomer()
        {
            var customer = gateway.Post(CreateStripeCustomerRequest());
            return customer;
        }

        protected async Task<StripeCustomer> CreateCustomerAsync()
        {
            var customer = await gateway.PostAsync(CreateStripeCustomerRequest());
            return customer;
        }

        private static CreateStripeCustomer CreateStripeCustomerRequest()
        {
            return new CreateStripeCustomer
            {
                AccountBalance = 10000,
                Card = new StripeCard
                {
                    Name = "Test Card",
                    Number = "4242424242424242",
                    Cvc = "123",
                    ExpMonth = 1,
                    ExpYear = 2020,
                    AddressLine1 = "1 Address Road",
                    AddressLine2 = "12345",
                    AddressZip = "City",
                    AddressState = "NY",
                    AddressCountry = "US",
                },
                Description = "Description",
                Email = "test@email.com",
            };
        }

        protected StripeCoupon CreateCoupon()
        {
            var coupon = gateway.Post(new CreateStripeCoupon
            {
                Id = "TEST-COUPON-01",
                Duration = StripeCouponDuration.repeating,
                PercentOff = 20,
                Currency = "usd",
                DurationInMonths = 2,
                RedeemBy = DateTime.UtcNow.AddYears(1),
                MaxRedemptions = 10,
            });
            return coupon;
        }

        protected StripeCoupon GetOrCreateCoupon()
        {
            try
            {
                return gateway.Get(new GetStripeCoupon { Id = "TEST-COUPON-01" });
            }
            catch (StripeException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return CreateCoupon();

                throw;
            }
        }

        protected StripePlan GetOrCreatePlan(string id = "TEST-PLAN-01")
        {
            try
            {
                return gateway.Get(new GetStripePlan { Id = id });
            }
            catch (StripeException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return CreatePlan(id);

                throw;
            }
        }

        protected StripePlan CreatePlan(string id = "TEST-PLAN-01")
        {
            var plan = gateway.Post(new CreateStripePlan
            {
                Id = id,
                Amount = 10000,
                Currency = "usd",
                Name = "Test Plan",
                Interval = StripePlanInterval.month,
                IntervalCount = 1,
            });
            return plan;
        }
    }
}