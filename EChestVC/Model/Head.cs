﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Model
{
    public class Head
    {
        public enum Target
        {
            Commit,
            Branch,
            Uninitialized
        }
        private Commit targetCommit;
        private Target targetType;
        
        public Head(Commit targetCommit)
        {
            targetType = Target.Commit;
            this.targetCommit = targetCommit;
        }

        public Head()
        {
            targetType = Target.Uninitialized;
            targetCommit = Commit.Null();
        }

        public Commit GetTarget()
        {
            return targetCommit;
        }
    }
}
