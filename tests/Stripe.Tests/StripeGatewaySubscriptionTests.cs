using NUnit.Framework;
using ServiceStack.Stripe;
using ServiceStack.Stripe.Types;
using ServiceStack.Text;

namespace Stripe.Tests
{
    /// <summary>
    /// https://stripe.com/docs/api/curl#subscriptions
    /// </summary>
    [TestFixture]
    public class StripeGatewaySubscriptionTests : TestsBase
    {
        [Test]
        public void Can_Subscribe_Customer()
        {
            var customer = CreateCustomer();
            var coupon = GetOrCreateCoupon();
            var plan = GetOrCreatePlan();

            var subscription = gateway.Post(new SubscribeStripeCustomer
            {
                CustomerId = customer.Id,
                Plan = plan.Id,
                Coupon = coupon.Id,
                Quantity = 1,
            });

            subscription.PrintDump();

            Assert.That(subscription.Id, Is.Not.Null);
            Assert.That(subscription.Customer, Is.EqualTo(customer.Id));
            Assert.That(subscription.Status, Is.EqualTo(StripeSubscriptionStatus.Active));
            Assert.That(subscription.Plan.Id, Is.EqualTo(plan.Id));
            Assert.That(subscription.Quantity, Is.EqualTo(1));
        }

        [Test]
        public void Can_Cancel_Subscription()
        {
            var customer = CreateCustomer();
            var plan = GetOrCreatePlan();

            var subscription = gateway.Post(new SubscribeStripeCustomer
            {
                CustomerId = customer.Id,
                Plan = plan.Id,
                Quantity = 1,
            });

            var cancelled = gateway.Delete(new CancelStripeSubscription
            {
                CustomerId = customer.Id,
                AtPeriodEnd = false,
            });

            Assert.That(cancelled.Customer, Is.EqualTo(customer.Id));
            Assert.That(cancelled.Id, Is.EqualTo(subscription.Id));
            Assert.That(cancelled.Status, Is.EqualTo(StripeSubscriptionStatus.Canceled));
        }
    }
}