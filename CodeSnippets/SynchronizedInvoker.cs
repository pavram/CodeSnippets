using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CodeSnippets
{

    public class SynchronizedInvoker
    {
        /// <summary>
        /// Allows a worker thread to more-easily raise an event on the UI.
        /// Thanks go to: http://stackoverflow.com/questions/1698889/raise-events-in-net-on-the-main-ui-thread
        /// Was the nicest answer.
        /// </summary>
        /// <param name="theEvent"></param>
        /// <param name="args"></param>
        public static void RaiseEventOnUIThread(Delegate theEvent, params object[] args)
        {
            foreach (Delegate d in theEvent.GetInvocationList())
            {
                ISynchronizeInvoke syncer = d.Target as ISynchronizeInvoke;
                if (syncer == null)
                {
                    d.DynamicInvoke(args);
                }
                else
                {
                    syncer.BeginInvoke(d, args);  // cleanup omitted
                }
            }
        }

        /// <summary>
        /// An example of an object that could need this.
        /// This object is a complicated system that performs a task on a background thread.
        /// The "Result" of the thread is pushed out to the rest of the world, and the helper function
        /// "RaiseEventOnUIThread" makes sure it gets executed on the thread that added the event callback function.
        /// </summary>
        internal class ExampleObject
        {
            public delegate void ExampleHandler(string param1);
            public event ExampleHandler ExampleEvent;

            private void OnExampleEvent(string BackgroundProcessingResults)
            {
                if (this.ExampleEvent != null)
                    SynchronizedInvoker.RaiseEventOnUIThread(this.ExampleEvent, BackgroundProcessingResults);
            }

            public void BackgroundTask()
            {
                string BackgroundResult;
                BackgroundResult = "Amazing";

                // You could replace the below with the code from OnExampleEvent
                // But this is generally a little cleaner and easier to read.
                OnExampleEvent(BackgroundResult);
            }
        }
        internal static void ExampleProgram()
        {
            // Assuming setting some variable is thread-sensitive.
            // Often the variable is the state or value of a UI object in a Forms application
            // since you can't change UI objects across threads.
            string ThreadSensitiveVariable;
            ExampleObject AnObject = new ExampleObject();

            // Generally speaking you should only do this once during initialization
            AnObject.ExampleEvent += Result => ThreadSensitiveVariable = Result;

            var backgroundTask = Task.Run(() => AnObject.BackgroundTask());

            // In a windows forms application, if this were a button event, you can just return now
            // That lets the UI thread keep chugging along.
            

        }
    }
}
