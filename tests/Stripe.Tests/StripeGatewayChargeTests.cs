using NUnit.Framework;
using ServiceStack.Stripe;
using ServiceStack.Text;

namespace Stripe.Tests
{
    /*
     * Charges 
     * https://stripe.com/docs/api/curl#charges
     */
    [TestFixture]
    public class StripeGatewayChargeTests : TestsBase
    {
        [Test]
        public void Can_Charge_Customer()
        {
            var customer = CreateCustomer();

            var charge = gateway.Post(new ChargeStripeCustomer
            {
                Amount = 100,
                Customer = customer.Id,
                Currency = "usd",
                Description = "Test Charge Customer",
            });

            charge.PrintDump();

            Assert.That(charge.Id, Is.Not.Null);
            Assert.That(charge.Customer, Is.EqualTo(customer.Id));
            Assert.That(charge.Amount, Is.EqualTo(100));
            Assert.That(charge.Card.Last4, Is.EqualTo("4242"));
            Assert.That(charge.Paid, Is.True);
        }

        [Test]
        public void Can_Get_Charge()
        {
            var customer = CreateCustomer();

            var charge = gateway.Post(new ChargeStripeCustomer
            {
                Amount = 100,
                Customer = customer.Id,
                Currency = "usd",
                Description = "Test Charge Customer",
            });

            charge = gateway.Get(new GetStripeCharge { ChargeId = charge.Id });
            charge.PrintDump();

            Assert.That(charge.Id, Is.Not.Null);
            Assert.That(charge.Customer, Is.EqualTo(customer.Id));
            Assert.That(charge.Amount, Is.EqualTo(100));
            Assert.That(charge.Card.Last4, Is.EqualTo("4242"));
            Assert.That(charge.Paid, Is.True);
        }

        [Test]
        public void Can_RefundCharge()
        {
            var customer = CreateCustomer();

            var charge = gateway.Post(new ChargeStripeCustomer
            {
                Amount = 100,
                Customer = customer.Id,
                Currency = "usd",
                Description = "Test Charge Customer",
            });

            //charge.PrintDump();

            Assert.That(charge.Id, Is.Not.Null);
            Assert.That(charge.Customer, Is.EqualTo(customer.Id));
            Assert.That(charge.Amount, Is.EqualTo(100));
            Assert.That(charge.Card.Last4, Is.EqualTo("4242"));
            Assert.That(charge.Paid, Is.True);

            var refundCharge = gateway.Post(new RefundStripeCharge
            {
                ChargeId = charge.Id,
            });

            refundCharge.PrintDump();

            Assert.That(refundCharge.Id, Is.Not.Null);
            Assert.That(refundCharge.Customer, Is.EqualTo(customer.Id));
            Assert.That(refundCharge.Amount, Is.EqualTo(100));
            Assert.That(refundCharge.Paid, Is.True);
            Assert.That(refundCharge.Refunded, Is.True);
            Assert.That(refundCharge.Refunds.Count, Is.EqualTo(1));
            Assert.That(refundCharge.Refunds[0].Amount, Is.EqualTo(100));
        }

        [Test]
        public void Can_CaptureCharge()
        {
            var customer = CreateCustomer();

            var charge = gateway.Post(new ChargeStripeCustomer
            {
                Amount = 100,
                Customer = customer.Id,
                Currency = "usd",
                Description = "Test Charge Customer",
                Capture = false,
            });

            Assert.That(charge.Paid, Is.True);
            Assert.That(charge.Captured, Is.False);

            var captureCharge = gateway.Post(new CaptureStripeCharge
                {
                    ChargeId = charge.Id,
                });

            captureCharge.PrintDump();

            Assert.That(captureCharge.Paid, Is.True);
            Assert.That(captureCharge.Captured, Is.True);
        }

        [Test]
        public void Can_List_all_Charges()
        {
            var customer = CreateCustomer();

            var charge = gateway.Post(new ChargeStripeCustomer
            {
                Amount = 100,
                Customer = customer.Id,
                Currency = "usd",
                Description = "Test Charge Customer",
                Capture = false,
            });

            var charges = gateway.Get(new GetStripeCharges());

            charges.PrintDump();

            Assert.That(charges.Count, Is.GreaterThan(0));
            Assert.That(charges.Data[0].Id, Is.Not.Null);
        }

        [Test]
        public void Can_List_Customer_Charges()
        {
            var customer = CreateCustomer();

            var charge = gateway.Post(new ChargeStripeCustomer
            {
                Amount = 100,
                Customer = customer.Id,
                Currency = "usd",
                Description = "Test Charge Customer",
                Capture = false,
            });

            var charges = gateway.Get(new GetStripeCharges
            {
                Customer = customer.Id,
            });

            charges.PrintDump();

            Assert.That(charges.Count, Is.EqualTo(1));
            Assert.That(charges.Data[0].Id, Is.Not.Null);
            Assert.That(charges.Data[0].Customer, Is.EqualTo(customer.Id));
        }
    }
}