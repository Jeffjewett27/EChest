using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Model
{
    class DiffChangelog
    {
        private Changelog common;
        private Changelog[] conflicts;

        public DiffChangelog(Changelog first, Changelog second)
        {
            DiffInit(new Changelog[] { first, second});
        }

        public DiffChangelog(Changelog[] changelogs)
        {
            DiffInit(changelogs);
        }

        /// <summary>
        /// Conflict types: files of same name and different hash
        /// </summary>
        /// <param name="changelogs"></param>
        private void DiffInit(Changelog[] changelogs)
        {

        }
    }
}
