using NUnit.Framework;
using ServiceStack.Stripe;
using ServiceStack.Stripe.Types;
using ServiceStack.Text;

namespace Stripe.Tests
{
    [TestFixture]
    public class StripeGatewayPlanTests : TestsBase
    {
        [Test]
        public void Can_Create_Plan()
        {
            var plan = GetOrCreatePlan();

            plan.PrintDump();

            Assert.That(plan.Id, Is.EqualTo("TEST-PLAN-01"));
            Assert.That(plan.Name, Is.EqualTo("Test Plan"));
            Assert.That(plan.Amount, Is.EqualTo(10000));
            Assert.That(plan.Interval, Is.EqualTo(StripePlanInterval.month));
        }

        [Test]
        public void Can_Get_Plan()
        {
            var plan = GetOrCreatePlan();

            plan = gateway.Get(new GetStripePlan { Id = plan.Id });

            Assert.That(plan.Id, Is.Not.Null);
        }

        [Test]
        public void Can_Update_Plan()
        {
            var plan = GetOrCreatePlan("NEW PLAN");

            var updatedPlan = gateway.Post(new UpdateStripePlan
            {
                Id = plan.Id,
                Name = "NEW PLAN UPDATED!",
            });

            Assert.That(updatedPlan.Name, Is.EqualTo("NEW PLAN UPDATED!"));
        }

        [Test]
        public void Can_Delete_All_Plans()
        {
            var plans = gateway.Get(new GetStripePlans { Limit = 100 });
            foreach (var plan in plans.Data)
            {
                gateway.Delete(new DeleteStripePlan { Id = plan.Id });
            }
        }

        [Test]
        public void Can_Get_All_Plans()
        {
            var plan = GetOrCreatePlan();

            var plans = gateway.Get(new GetStripePlans { Limit = 20 });

            Assert.That(plans.Data.Count, Is.GreaterThan(0));
            Assert.That(plans.Data[0].Id, Is.Not.Null);
        }
    }
}