using System;
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
        private readonly Commit targetCommit;
        private readonly Branch targetBranch;
        private readonly Target targetType;
        private readonly string targetHash;

        public Branch TargetBranch {
            get
            {
                if (targetType != Target.Branch)
                {
                    throw new InvalidOperationException("Head must be of type Target.Branch");
                }
                return targetBranch;
            }
        }

        public Target TargetType => targetType;
        public string TargetHash => targetHash;
        
        public Head(Commit targetCommit)
        {
            targetType = Target.Commit;
            targetHash = targetCommit.Hash;
            this.targetCommit = targetCommit;
        }

        public Head(Branch targetBranch)
        {
            targetType = Target.Branch;
            targetHash = targetBranch.Name;
            this.targetBranch = targetBranch;
        }

        public Head()
        {
            targetType = Target.Uninitialized;
            targetCommit = Commit.Null();
        }

        public Commit GetTarget()
        {
            if (targetType == Target.Branch)
            {
                return targetBranch.Target;
            }
            return targetCommit;
        }
    }
}
