using System;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem.Tests
{
    internal class MockDateTime : IDateTimeService
    {
        private DateTime CurrentDateTime
        {
            get;
            set;
        }

        public MockDateTime(DateTime currentDateTime)
        {
            CurrentDateTime = currentDateTime;
        }

        public DateTime GetCurrentDateTime()
        {
            return CurrentDateTime;
        }
    }
}
