using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseWork.Distributions;

using MathNet.Numerics.Distributions;

namespace CourseWork
{
	public class ModelFactory
	{
		public const double ADDITIONAL_RUNWAY_COST = 3000000.0;

		public static (PetriNet, Func<double, double>) GetNet(int runways)
		{

			var p1 = new Place("Plane generator", 1);

			var refuelingTimerStart = new Place("Refueling timer start");
			var refuelingTimerReset = new Place("Refueling timer reset");
			var refuelingTimerEnd = new Place("Refueling timer end");
			var refuelingTimerReseted = new Place("Reseted refueling timers");

			var leaveTimerStart = new Place("Leave timer start");
			var leaveTimerEnd = new Place("Leave timer end");
			var leaveTimerReset = new Place("Leave timer reset");
			var leaveTimerReseted = new Place("Reseted leave timers");

			var refuelings = new Place("Refuelings");
			var refuelingExpenses = new Place("Refueling expenses");
			var leaves = new Place("Leaves");

			var freeRunways = new Place("Free runways", runways);

			var planeQueue_refueled = new Place("Plane queue (refueled)");

			var planeQueue = new TypeDeterminingPlace("Plane queue", freeRunways, planeQueue_refueled);

			var landing1Place = new Place("Landing 1");
			var landing2Place = new Place("Landing 2");

			var landedPlanes = new Place("Landed planes");

			var income1 = new Place("Income for landings with waiting");
			var income2 = new Place("Income for landings without waiting");

			var generator = new Transition(1, [p1], [p1, planeQueue, leaveTimerStart, refuelingTimerStart])
			{
				Generator = new DistributionAdapter(new Exponential(1.0 / 6.0))
			};

			var refuelTimer = new Transition(2, [refuelingTimerStart], [refuelingTimerEnd])
			{
				Generator = new DistributionAdapter(new ContinuousUniform(60, 80))
			};

			var refuelTimerReseter = new Transition(3, [refuelingTimerEnd, refuelingTimerReset], [refuelingTimerReseted], 1);

			var refueling = new Transition(4, [planeQueue, refuelingTimerEnd], [refuelings, planeQueue_refueled], 0, planeQueue);

			var refuelingExpenseCounter = new OutputTransition(5, refuelings, refuelingExpenses, 1200, 800);

			var leaveTimer = new Transition(6, [leaveTimerStart], [leaveTimerEnd])
			{
				Generator = new ConstantGenerator(140.0)
			};

			var leaveTimerReseter = new Transition(7, [leaveTimerEnd, leaveTimerReset], [leaveTimerReseted], 1);

			var leaving = new Transition(8, [planeQueue_refueled, leaveTimerEnd], [leaves]);

			var landing1 = new Transition(9, [planeQueue, freeRunways], [refuelingTimerReset, leaveTimerReset, landing1Place], 1, planeQueue);
			var landing2 = new Transition(10, [planeQueue_refueled, freeRunways], [leaveTimerReset, landing2Place], 1, planeQueue_refueled);

			var countIncomeForLandingWithWaiting = new OutputTransition(11, landedPlanes, income1, 1600, 1400, TokenType.LandingWithWaiting);
			var countIncomeForLandingWithotWaiting = new OutputTransition(12, landedPlanes, income2, 2000, TokenType.LandingWithoutWaiting);

			var releaseRunway1 = new Transition(13, [landing1Place], [freeRunways, landedPlanes], 0, landing1Place)
			{
				Generator = new ConstantGenerator(17)
			};

			var releaseRunway2 = new Transition(14, [landing2Place], [freeRunways, landedPlanes], 0, landing2Place)
			{
				Generator = new ConstantGenerator(17)
			};

			var model = new PetriNet([
				generator,
				refuelTimer,
				refuelTimerReseter,
				refueling,
				leaveTimer,
				leaveTimerReseter,
				leaving,
				landing1,
				landing2,
				releaseRunway1,
				releaseRunway2,
				refuelingExpenseCounter,
				countIncomeForLandingWithotWaiting,
				countIncomeForLandingWithWaiting
			], [
				p1,
				refuelingTimerStart,
				refuelingTimerReset,
				refuelingTimerEnd,
				refuelingTimerReseted,
				leaveTimerStart,
				leaveTimerEnd,
				leaveTimerReset,
				leaveTimerReseted,
				refuelings,
				refuelingExpenses,
				leaves,
				freeRunways,
				planeQueue_refueled,
				planeQueue,
				landing1Place,
				landing2Place,
				landedPlanes,
				income1,
				income2
			], false);
			return (model, GetPaybackPeriodFunction(income1, income2, refuelingExpenses, runways - 2));
		}

		private static Func<double, double> GetPaybackPeriodFunction(Place incomeWithWaiting, Place incomeWithoutWaiting, Place refuelingExpenses, int countOfRunways)
		{
			return (tPeriod) =>
			{
				double profit = incomeWithWaiting.TokenCount + incomeWithoutWaiting.TokenCount - refuelingExpenses.TokenCount;
				double runwaysExpenses = countOfRunways * ADDITIONAL_RUNWAY_COST;

				if (profit <= 0.0)
					return Double.PositiveInfinity;
				return runwaysExpenses / (profit * tPeriod);
			};
		}
	}
}
