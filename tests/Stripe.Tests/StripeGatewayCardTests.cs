using NUnit.Framework;
using ServiceStack.Stripe;
using ServiceStack.Stripe.Types;
using ServiceStack.Text;

namespace Stripe.Tests
{
    /*
     * Cards
     * https://stripe.com/docs/api/curl#cards
     */
    [TestFixture]
    public class StripeGatewayCardTests : TestsBase
    {
        [Test]
        public void Can_Add_New_Card_to_Customer()
        {
            var customer = CreateCustomer();

            Assert.That(customer.Cards.Count, Is.EqualTo(1));

            var card = gateway.Post(new CreateStripeCard
            {
                CustomerId = customer.Id,
                Card = new StripeCard
                {
                    Name = "Test Card 2",
                    Number = "5555555555554444",
                    Cvc = "456",
                    ExpMonth = 1,
                    ExpYear = 2016,
                    AddressLine1 = "1 Address Road",
                    AddressLine2 = "12345",
                    AddressZip = "City",
                    AddressState = "NY",
                    AddressCountry = "US",
                },
            });

            card.PrintDump();

            Assert.That(card.Id, Is.Not.Null);

            customer = gateway.Get(new GetStripeCustomer { Id = customer.Id });

            Assert.That(customer.Cards.Count, Is.EqualTo(2));
        }

        [Test]
        public void Can_Get_Customer_Card()
        {
            var customer = CreateCustomer();
            var card = gateway.Post(new CreateStripeCard
            {
                CustomerId = customer.Id,
                Card = new StripeCard
                {
                    Name = "Test Card 2",
                    Number = "5555555555554444",
                    Cvc = "456",
                    ExpMonth = 1,
                    ExpYear = 2016,
                    AddressLine1 = "1 Address Road",
                    AddressLine2 = "12345",
                    AddressZip = "City",
                    AddressState = "NY",
                    AddressCountry = "US",
                },
            });

            Assert.That(card.Name, Is.EqualTo("Test Card 2"));
            Assert.That(card.Last4, Is.EqualTo("4444"));

            card = gateway.Get(new GetStripeCard
            {
                CustomerId = customer.Id,
                CardId = card.Id,
            });

            card.PrintDump();

            Assert.That(card.Name, Is.EqualTo("Test Card 2"));
            Assert.That(card.Last4, Is.EqualTo("4444"));
        }

        [Test]
        public void Can_Delete_Customer_Card()
        {
            var customer = CreateCustomer();

            Assert.That(customer.Cards.Count, Is.EqualTo(1));

            var deletedRef = gateway.Delete(new DeleteStripeCard
            {
                CustomerId = customer.Id,
                CardId = customer.Cards.Data[0].Id,
            });

            Assert.That(deletedRef.Id, Is.EqualTo(customer.Cards.Data[0].Id));
            Assert.That(deletedRef.Deleted, Is.True);

            customer = gateway.Get(new GetStripeCustomer { Id = customer.Id });

            Assert.That(customer.Cards.Count, Is.EqualTo(0));
        }

        [Test]
        public void Can_Get_All_Customer_Cards()
        {
            var customer = CreateCustomer();
            var card = gateway.Post(new CreateStripeCard
            {
                CustomerId = customer.Id,
                Card = new StripeCard
                {
                    Name = "Test Card 2",
                    Number = "5555555555554444",
                    Cvc = "456",
                    ExpMonth = 1,
                    ExpYear = 2016,
                    AddressLine1 = "1 Address Road",
                    AddressLine2 = "12345",
                    AddressZip = "City",
                    AddressState = "NY",
                    AddressCountry = "US",
                },
            });

            var cards = gateway.Get(new GetStripeCards { CustomerId = customer.Id });

            Assert.That(cards.Count, Is.EqualTo(2));
        }
    }
}