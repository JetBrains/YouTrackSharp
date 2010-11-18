// Type: JetBrains.TreeModels.TreeSimpleModel
// Assembly: JetBrains.Platform.ReSharper.Util, Version=5.1.1753.1, Culture=neutral, PublicKeyToken=1010a0d8d6380325
// Assembly location: C:\Program Files (x86)\JetBrains\ReSharper\v5.1\Bin\JetBrains.Platform.ReSharper.Util.dll

using System.Collections.Generic;

namespace JetBrains.TreeModels
{
    public class TreeSimpleModel : TreeModel
    {
        protected internal override TreeModelNode CreateNode(TreeModelNode parent, object value);
        public void Insert(object parentValue, object childValue);
        public new void Remove(object childValue);
        protected override void PerformUpdate();

        #region Nested type: TreeSimpleNode

        protected class TreeSimpleNode : TreeModelNode
        {
            public TreeSimpleNode(TreeSimpleModel model, TreeModelNode parent, object value);
            public override IList<TreeModelNode> ChildrenUnsorted { get; }
        }

        #endregion
    }
}
