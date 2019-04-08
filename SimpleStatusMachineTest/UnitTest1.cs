using SimpleStatusMachine;
using static System.Console;
using Xunit;

namespace SimpleStatusMachineTest
{
    public class UnitTest1
    {
        public UnitTest1()
        {
            sm = new StatusMachine<long>();
            sm.InitStatus(1);
            sm.SetLoopSpeed(500);

            sm.AddStatus(1, () => { WriteLine("ent1"); }, () => { WriteLine("upadate1 next 2"); return 2; }, () => { WriteLine("exit1"); });
            sm.AddStatus(2, () => { WriteLine("ent2"); }, () => { WriteLine("upadate2 next 3"); return 3; }, () => { WriteLine("exit2"); });
            sm.AddStatus(3, () => { WriteLine("ent3"); }, () => { WriteLine("upadate3 next 4"); return 4; }, () => { WriteLine("exit3"); });
            sm.AddStatus(4, () => { WriteLine("ent4"); }, () => { WriteLine("upadate4 next 5"); return 5; }, () => { WriteLine("exit4"); });
            sm.AddStatus(5, () => { WriteLine("ent5"); }, () => 6, () => { WriteLine("exit5"); });
        }
        IStatusMachine<long> sm;
        [Fact(DisplayName ="1 to 2")]
        public void Test1()
        {
            sm.Update();
            Assert.Equal(2, sm.GetCurrentStatus());
        }
        [Fact(DisplayName ="2 to 3")]
        public void Test2()
        {
            sm.Update();
            sm.Update();
            Assert.Equal(3, sm.GetCurrentStatus());
        }
        [Fact(DisplayName = "nobody status id")]
        public void Test3()
        {
            sm.Update();
            sm.Update();
            sm.Update();
            sm.Update();
            sm.Update();
            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(()=>sm.Update());
        }
    }
}
