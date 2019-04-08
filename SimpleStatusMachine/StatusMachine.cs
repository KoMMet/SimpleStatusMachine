using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleStatusMachine
{
    public interface IStatusMachine<T>
    {
        void AddStatus(T status, Action entry, Func<T> update, Action exit);
        void Update(bool isSleep = false);
        void InitStatus(T id);
        void SetLoop();
        void SetLoopSpeed(int time);
        T GetCurrentStatus();
    }

    public class StatusMachine<T> : IStatusMachine<T>
    {
        private Dictionary<T, StatusMachineSorce<T>> status = new Dictionary<T, StatusMachineSorce<T>>();
        private T id;
        private int time;
        private bool canNext = true;
        public void AddStatus(T status, Action entry, Func<T> update, Action exit)
        {
            var sms = new StatusMachineSorce<T> { ent = entry ?? (() => { }), update = update ?? (() => default), exit = exit ?? (() => { })};
            this.status.Add(status, sms);
        }

        public void Update(bool isSleep = false)
        {
            if(isSleep)
                Task.Delay(time);
            if (canNext) status[id].ent();
            canNext = parseUpdate(this.id);
        }

        public void SetLoop()
        {
            while (true)
                Update(true);
        }

        public async Task SetLoopAsync()
        {
            await Task.Run(() => SetLoop());
        }

        private bool parseUpdate(T id)
        {
            this.id = status[this.id].update();
            if (this.id.Equals(id))
            {
                status[id].exit();
                return true;
            }
            return false;
        }

        public void InitStatus(T id)
        {
            this.id = id;
        }

        public void SetLoopSpeed(int time)
        {
            this.time = time;
        }

        public T GetCurrentStatus()
        {
            return id;
        }
    }

    internal class StatusMachineSorce<T>
    {
        public Action ent { get; set; }
        public Func<T> update { get; set; }
        public Action exit { get; set; }
    }
}
